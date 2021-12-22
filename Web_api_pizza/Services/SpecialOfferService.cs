﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface ISpecialOfferService
    {
        public SpecialOfferDTO GetSpecialOffer(int id);
        public List<SpecialOfferDTO> GetAllSpecialOffers(SpecialOfferFilter filter);
        public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer);
        public OperationResult EditSpecialOffer(SpecialOfferDTO specialOffer);
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
                .AsQueryable();

            specialOffers = filter.Filters(specialOffers);

            var specialOffersEntity = specialOffers.ToList();

            var specialOffersDTO = _mapper.Map<List<SpecialOfferDTO>>(specialOffersEntity);
            return specialOffersDTO;
        }

        public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer)
        {
            var result = new OperationResult(false);

            var specialOfferFromDB = _context.Offers
                .Where(x => x.PromoCode == specialOffer.PromoCode)
                .FirstOrDefault();
            if (specialOfferFromDB != null)
            {
                result.Message = "Акция с таким промокодом уже существует";
                return result;
            }
            
            var newSpecialOffer = new SpecialOfferEntity();

            if (specialOffer.TypeOffer == TypeOfferEnum.GeneralDiscount)
            {

                result = ValidateGeneralDiscount(specialOffer);
                if (result.IsSuccess == false)
                {
                    return result;
                }
                newSpecialOffer = _mapper.Map(specialOffer, newSpecialOffer);
            }
            else
            {
                //в методе Edit повпоряется эта проверка
                var mainDishEntity = _context.Dishes
                    .Where(x => x.Id == specialOffer.MainDish.Id)
                    .FirstOrDefault();
                var extraDishEntity = _context.Dishes
                .Where(x => x.Id == specialOffer.ExtraDish.Id)
                .FirstOrDefault();

                result = ValidateOffersWithDishes(specialOffer, mainDishEntity, extraDishEntity);

                if (result.IsSuccess == false)
                {
                    return result;
                }
                newSpecialOffer = _mapper.Map(specialOffer,newSpecialOffer);
                newSpecialOffer.MainDishId = mainDishEntity.Id;
                newSpecialOffer.ExtraDishId = extraDishEntity.Id;
            }
            _context.Offers.Add(newSpecialOffer);
            _context.SaveChanges();
            result.Message = "Акция добавлена";
            return result;
        }

        public OperationResult EditSpecialOffer(SpecialOfferDTO specialOffer)
        {
            var specialOfferEntity = _context.Offers
                .Where(so => so.Id == specialOffer.Id)
                .Include(so => so.MainDish)
                .FirstOrDefault();
            if (specialOfferEntity == null)
                return null;

            var result = new OperationResult(false);

            if(specialOffer.TypeOffer == TypeOfferEnum.GeneralDiscount)
            {
                result = ValidateGeneralDiscount(specialOffer);
                if (result.IsSuccess == false)
                {
                    return result;
                }
            }
            else
            {
                var mainDishEntity = _context.Dishes
                    .Where(x => x.Id == specialOffer.MainDish.Id)
                    .FirstOrDefault();
                var extraDishEntity = _context.Dishes
                .Where(x => x.Id == specialOffer.ExtraDish.Id)
                .FirstOrDefault();

                result = ValidateOffersWithDishes(specialOffer, mainDishEntity, extraDishEntity);

                if (result.IsSuccess == false)
                {
                    return result;
                }
                specialOfferEntity.MainDishId = mainDishEntity.Id;
                specialOfferEntity.ExtraDishId = extraDishEntity.Id;
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

        private OperationResult ValidateGeneralDiscount(SpecialOfferDTO specialOffer)
        {
            Console.WriteLine("скидка");
            var result = new OperationResult(false);
            if (specialOffer.Discount < 5 || specialOffer.Discount > 20)
            {
                result.Message = $"Недопустимый размер скидки — \"{specialOffer.Discount}%\"";
                return result;
            }

            Console.WriteLine("основное блюдо");

            if (specialOffer.MainDish != null)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" недолжно быть основных блюд";
                return result;
            }


            Console.WriteLine("доп блюда");

            if (specialOffer.ExtraDish != null)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" недолжно быть доп блюд";
                return result;
            }


            Console.WriteLine("количество основных");

            if (specialOffer.RequiredNumberOfDish != 0)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" необходимое число блюд должно равняться 0";
                return result;
            }

            Console.WriteLine("количество доп");

            if (specialOffer.NumberOfExtraDish != 0)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" число доп блюд должно равняться 0";
                return result;
            }
            

            result.IsSuccess = true;
            result.Message = "Успешно";

            return result;
        }

        private OperationResult ValidateOffersWithDishes(SpecialOfferDTO specialOffer, DishEntity mainDish, DishEntity extraDish)
        {
            var result = new OperationResult(false);
            if(specialOffer.Discount != 0)
            {
                result.Message = $"У акции типа {specialOffer.TypeOffer} не должно быть скидки";
                return result;
            }
            if (specialOffer.MainDish == null || mainDish == null)
            {
                result.Message = "Список основных блюд не соответствует блюдам из БД";
                return result;
            }

            if (specialOffer.ExtraDish == null || extraDish == null)
            {
                result.Message = "Дополнительное блюдо не соответствует блюдам из БД";
                return result;
            }

            if (specialOffer.RequiredNumberOfDish < 2 || specialOffer.RequiredNumberOfDish > 10)
            {
                result.Message = $"Недопустимое значение RequiredNumberOfDish - {specialOffer.RequiredNumberOfDish}";
                return result;
            }

            if (specialOffer.NumberOfExtraDish < 1 || specialOffer.NumberOfExtraDish > 5)
            {
                result.Message = $"Недопустимое значение NumberOfExtraDish — {specialOffer.NumberOfExtraDish}";
                return result;
            }

            result.IsSuccess = true;
            result.Message = "Успешно";
            return result;
        }
    }
}
