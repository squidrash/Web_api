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
        /// <summary>
        /// Получение данных конкретной акции
        /// </summary>
        /// <param name="id"> Id акции</param>
        /// <returns>Конкретную акцию</returns>
        public SpecialOfferDTO GetSpecialOffer(int id);

        /// <summary>
        /// Получения списка акций
        /// </summary>
        /// <param name="filter">Опциональный параметр. Фильтрация по типу акции</param>
        /// <returns>Список акций</returns>
        public List<SpecialOfferDTO> GetAllSpecialOffers(SpecialOfferFilter filter);

        /// <summary>
        /// Добавление новой акции
        /// </summary>
        /// <param name="specialOffer">Данные акции</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer);

        /// <summary>
        /// Изменение данных акции
        /// </summary>
        /// <param name="specialOffer">Данные акции</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult EditSpecialOffer(SpecialOfferDTO specialOffer);

        /// <summary>
        /// Проверка соответствия списка блюд условиям акции
        /// </summary>
        /// <param name="dishes">Список проверяемых блюд</param>
        /// <param name="promoCode">промокод акции по которой будет проводиться проверка</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult CheckComplianceSpecialOffer(List<DishDTO> dishes, string promoCode);

        /// <summary>
        /// Удаление акции
        /// </summary>
        /// <param name="specialOfferId">Id акции</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult DeleteSpecialOffer(int specialOfferId);
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
                .Include(so => so.MainDish)
                .Include(so => so.ExtraDish)
                .FirstOrDefault(so => so.Id == id);
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
                .FirstOrDefault(x => x.PromoCode == specialOffer.PromoCode);

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
                .FirstOrDefault(so => so.Id == specialOffer.Id);
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

        public OperationResult DeleteSpecialOffer(int specialOfferId)
        {
            var specialOfferEntity = _context.Offers
                .FirstOrDefault(so => so.Id == specialOfferId);
            if (specialOfferEntity == null)
                return null;

            _context.Offers.Remove(specialOfferEntity);
            _context.SaveChanges();

            var result = new OperationResult(true, "Акция удалена");
            return result;
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
                   .FirstOrDefault(x => x.Id == specialOffer.MainDish.Id);

                extraDish = _context.Dishes
                   .FirstOrDefault(x => x.Id == specialOffer.ExtraDish.Id);
            }
            validationContext.SetStrategy(strategy);
            operationResult = validationContext.ValidateOffer(specialOffer, mainDish, extraDish);

            var result = new ResultOfferValidation(operationResult.IsSuccess, operationResult.Message, mainDish, extraDish);

            return result;
        }
    }
}
