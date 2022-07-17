using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.SpecialOfferFactory;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;
using Web_api_pizza.ValidateOfferStrategy;
using Web_api_pizza.ValidationOfferStrategy;
using Web_api_pizza.ValidationOfferStrategy.Adapter;
using Web_api_pizza.ValidationOfferStrategy.TemplateMethod;

namespace Web_api_pizza.Services
{
    public interface ISpecialOfferService
    {
        public SpecialOfferDTO GetSpecialOffer(int id);
        public List<SpecialOfferDTO> GetAllSpecialOffers(SpecialOfferFilter filter);
        public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer);
        public OperationResult EditSpecialOffer(SpecialOfferDTO specialOffer);
        public OperationResult CheckComplianceSpecialOffer(List<DishDTO> dishes, string promoCode);

        public string DeleteSpecialOffer(int specialOfferId);
    }

    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;

        public SpecialOfferService(IMapper mapper, PizzaDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public SpecialOfferDTO GetSpecialOffer(int id)
        {
            var specialOffer = _context.Offers
                .Where(so => so.Id == id)
                .Include(so => so.MainDish)
                .Include(so => so.ExtraDish)
                .FirstOrDefault();
            var specialOfferDTO = _mapper.Map<SpecialOfferDTO>(specialOffer);
            return specialOfferDTO;

        }

        public List<SpecialOfferDTO> GetAllSpecialOffers(SpecialOfferFilter filter)
        {
            var specialOffers = _context.Offers
                .Include(so => so.MainDish)
                .Include(so => so.ExtraDish)
                .OrderBy(so => so.Id)
                .AsQueryable();

            specialOffers = filter.Filters(specialOffers);

            var specialOffersEntity = specialOffers.ToList();

            var specialOffersDTO = _mapper.Map<List<SpecialOfferDTO>>(specialOffersEntity);
            return specialOffersDTO;
        }

        public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer)
        {
            var result = new OperationResult(false);

            var specialOfferEntity = _context.Offers
                .Where(x => x.PromoCode == specialOffer.PromoCode)
                .FirstOrDefault();

            if (specialOfferEntity != null)
            {
                result.Message = "Акция с таким промокодом уже существует";
                return result;
            }

            var validationResult = ValidateOffer(specialOffer);
            if(validationResult.IsSuccess == false)
            {
                return validationResult;
            }

            var newSpecialOffer = new SpecialOfferEntity();

            if (validationResult.MainDish != null && validationResult.ExtraDish != null)
            {
                newSpecialOffer.MainDishId = validationResult.MainDish.Id;
                newSpecialOffer.ExtraDishId = validationResult.ExtraDish.Id;
            }
            
            newSpecialOffer = _mapper.Map(specialOffer, newSpecialOffer);
            _context.Offers.Add(newSpecialOffer);
            _context.SaveChanges();
            result.Message = "Акция добавлена";
            return result;
        }

        public OperationResult EditSpecialOffer(SpecialOfferDTO specialOffer)
        {
            var specialOfferEntity = _context.Offers
                .Where(so => so.Id == specialOffer.Id)
                .FirstOrDefault();
            if (specialOfferEntity == null)
                return null;

            var result = new OperationResult(false);

            var validationResult = ValidateOffer(specialOffer);
            if (validationResult.IsSuccess == false)
            {
                return validationResult;
            }

            if (validationResult.MainDish != null && validationResult.ExtraDish != null)
            {
                specialOfferEntity.MainDishId = validationResult.MainDish.Id;
                specialOfferEntity.ExtraDishId = validationResult.ExtraDish.Id;
            }

            specialOfferEntity = _mapper.Map(specialOffer, specialOfferEntity);
            
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Изменения сохранены";
            return result;
        }

        public string DeleteSpecialOffer(int specialOfferId)
        {
            string message;

            var specialOfferEntity = _context.Offers
                .Where(so => so.Id == specialOfferId)
                .FirstOrDefault();
            if (specialOfferEntity == null)
                return null;

            _context.Offers.Remove(specialOfferEntity);
            _context.SaveChanges();

            message = "Акция удалена";
            return message;
        }

        private readonly Dictionary<TypeOfferEnum, StrategyFactory> StrategyFactoryDic = new Dictionary<TypeOfferEnum, StrategyFactory>
        {
            { TypeOfferEnum.GeneralDiscount, new GeneralDiscountCreator() },
            { TypeOfferEnum.ExtraDish, new ExtraDishCreator() },
            { TypeOfferEnum.ThreeForPriceTwo, new ThreeForPriceTwoCreator() }
        };

        public OperationResult CheckComplianceSpecialOffer(List<DishDTO> dishes, string promoCode)
        {
            var specialOfferEntity = _context.Offers
                .FirstOrDefault(x => x.PromoCode == promoCode);
            if (specialOfferEntity == null)
            {
                return new OperationResult(false, "Акции с таким промокодом не существует");
            }

            var strategyContext = StrategyFactoryDic[specialOfferEntity.TypeOffer].CreateStrategy();
            var complianceResult = strategyContext.CheckComplianceSpecialOffer(dishes, specialOfferEntity);

            if (complianceResult == -1)
            {
                return new OperationResult(false, "Не соответствует условиям акции");
            }

            return new ResultOfferCheck(true, "Промокод применен", complianceResult);
        }

        private ResultOfferValidation ValidateOffer(SpecialOfferDTO specialOffer)
        {
            var operationResult = new OperationResult(false);

            var validationContext = ValidationOfferContext.GetInstance();

            IValidationOfferStrategy strategy;

            switch (specialOffer.TypeOffer)
            {
                case TypeOfferEnum.GeneralDiscount:
                    var adapteeStrategy = new ValidationGeneralDiscount();
                    strategy = new ValidationStrategyAdapter(adapteeStrategy);
                    break;
                case TypeOfferEnum.ExtraDish:
                    strategy = new ValidationExtraDishes();
                    break;
                case TypeOfferEnum.ThreeForPriceTwo:
                    strategy = new ValidationThreeForPriceTwo();
                    break;
                default:
                    operationResult.Message = "Неизвестный тип акции";
                    return (ResultOfferValidation)operationResult;
            }

            DishEntity mainDish = null;
            DishEntity extraDish = null;

            var anyDishesOnOffer = specialOffer.MainDish != null && specialOffer.ExtraDish != null;

            if (anyDishesOnOffer)
            {
                mainDish = _context.Dishes
                   .Where(x => x.Id == specialOffer.MainDish.Id)
                   .FirstOrDefault();

                extraDish = _context.Dishes
                   .Where(x => x.Id == specialOffer.ExtraDish.Id)
                   .FirstOrDefault();
            }
            validationContext.SetStrategy(strategy);
            operationResult = validationContext.ValidateOffer(specialOffer, mainDish, extraDish);

            var result = new ResultOfferValidation(operationResult.IsSuccess, operationResult.Message, mainDish, extraDish);

            return result;
        }
    }
}
