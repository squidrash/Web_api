using System;
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
                .Include(so => so.MainDishes)
                .Include(so => so.ExtraDish)
                .FirstOrDefault();
            var specialOfferDTO = _mapper.Map<SpecialOfferDTO>(specialOffer);
            return specialOfferDTO;

        }
        public List<SpecialOfferDTO> GetAllSpecialOffers(SpecialOfferFilter filter)
        {
            var specialOffers = _context.Offers
                .Include(so => so.MainDishes)
                .Include(so => so.ExtraDish)
                .AsQueryable();

            specialOffers = filter.Filters(specialOffers);

            List<SpecialOfferEntity> specialOffersEntity;
            specialOffersEntity = specialOffers.ToList();

            var specialOffersDTO = _mapper.Map<List<SpecialOfferDTO>>(specialOffersEntity);
            return specialOffersDTO;
        }

        //public OperationResult AddNewSpecialOffer(SpecialOfferDTO specialOffer)
        //{
        //    var result = new OperationResult(false);

        //    var specialOfferFromDB = _context.Offers
        //        .Where(x => x.PromoCode == specialOffer.PromoCode)
        //        .FirstOrDefault();
        //    if (specialOfferFromDB != null)
        //    {
        //        result.Message = "Акция с таким промокодом уже существует";
        //        return result;
        //    }
        //    var newSpecialOffer = new SpecialOfferEntity()
        //    {
        //        Name = specialOffer.Name,
        //        Description = specialOffer.Description,
        //        TypeOffer = specialOffer.TypeOffer,
        //        PromoCode = specialOffer.PromoCode
        //    };

        //    if (specialOffer.TypeOffer == TypeOfferEnum.GeneralDiscount)
        //    {

        //        result = ValidateGeneralDiscount(specialOffer);
        //        if(result.IsSuccess == false)
        //        {
        //            return result;
        //        }
        //        newSpecialOffer.Discount = specialOffer.Discount;
        //    }
        //    else
        //    {
        //        //if (specialOffer.MainDishes.Count == 0 )
        //        //{
        //        //    message = "NullMainDishes";
        //        //    return message;
        //        //}

        //        //if (specialOffer.ExtraDish == null)
        //        //{
        //        //    message = "NullExtraDish";
        //        //    return message;
        //        //}

        //        //var dishEntity = _context.Dishes
        //        //    .Where(x => specialOffer.MainDishes.Select(y => y.Id.Value)
        //        //                      .Contains(x.Id))
        //        //    .ToList();


        //        //if (dishEntity.Count != specialOffer.MainDishes.Count)
        //        //{
        //        //    message = "NullMainDishes";
        //        //    return message;
        //        //}

        //        //newSpecialOffer.MainDishes = dishEntity;

        //        //var extraDishEntity = _context.Dishes
        //        //    .Where(x => x.Id == specialOffer.ExtraDish.Id)
        //        //    .FirstOrDefault();
        //        //if (extraDishEntity == null)
        //        //{
        //        //    message = "NullExtraDish";
        //        //    return message;
        //        //}
        //        //newSpecialOffer.ExtraDish = extraDishEntity;


        //        //if (specialOffer.RequiredNumberOfDish <= 2)
        //        //{
        //        //    newSpecialOffer.RequiredNumberOfDish = 2;
        //        //}
        //        //else
        //        //{
        //        //    newSpecialOffer.RequiredNumberOfDish = specialOffer.RequiredNumberOfDish;
        //        //}

        //        //if (specialOffer.NumberOfExtraDish <= 1)
        //        //{
        //        //    newSpecialOffer.NumberOfExtraDish = 1;
        //        //}
        //        //else
        //        //{
        //        //    newSpecialOffer.NumberOfExtraDish = specialOffer.NumberOfExtraDish;
        //        //}

        //        //через метод
        //        //в методе Edit повпоряется эта проверка
        //        var dishEntity = _context.Dishes
        //            .Where(x => specialOffer.MainDishes.Select(y => y.Id.Value)
        //                              .Contains(x.Id))
        //            .ToList();
        //        var extraDishEntity = _context.Dishes
        //        .Where(x => x.Id == specialOffer.ExtraDish.Id)
        //        .FirstOrDefault();

        //        result = ValidateOffersWithDishes(specialOffer, dishEntity, extraDishEntity);

        //        if(result.IsSuccess == false)
        //        {
        //            return result;
        //        }

        //        newSpecialOffer.MainDishes = dishEntity;
        //        newSpecialOffer.ExtraDish = extraDishEntity;
        //        newSpecialOffer.RequiredNumberOfDish = specialOffer.RequiredNumberOfDish;
        //        newSpecialOffer.NumberOfExtraDish = specialOffer.NumberOfExtraDish;
        //        //
        //    }

        //    //_context.Offers.Add(newSpecialOffer);
        //    //_context.SaveChanges();
        //    result.Message = "Акция добавлена";
        //    return result;
        //}

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
                var dishEntity = _context.Dishes
                    .Where(x => specialOffer.MainDishes.Select(y => y.Id.Value)
                                      .Contains(x.Id))
                    .ToList();
                var extraDishEntity = _context.Dishes
                .Where(x => x.Id == specialOffer.ExtraDish.Id)
                .FirstOrDefault();

                result = ValidateOffersWithDishes(specialOffer, dishEntity, extraDishEntity);

                if (result.IsSuccess == false)
                {
                    return result;
                }
                newSpecialOffer = _mapper.Map(specialOffer,newSpecialOffer);
                newSpecialOffer.MainDishes = dishEntity;
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
                .Include(so => so.MainDishes)
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
                var dishEntity = _context.Dishes
                    .Where(x => specialOffer.MainDishes.Select(y => y.Id.Value)
                                      .Contains(x.Id))
                    .ToList();
                var extraDishEntity = _context.Dishes
                .Where(x => x.Id == specialOffer.ExtraDish.Id)
                .FirstOrDefault();

                result = ValidateOffersWithDishes(specialOffer, dishEntity, extraDishEntity);

                if (result.IsSuccess == false)
                {
                    return result;
                }
                specialOfferEntity.MainDishes = dishEntity;
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

            try
            {
                Console.WriteLine("основные блюда");

                if (specialOffer.MainDishes.Count > 0)
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result.Message = e.Message;
                return result;
            }

            result.IsSuccess = true;
            result.Message = "Успешно";

            return result;
        }

        private OperationResult ValidateOffersWithDishes(SpecialOfferDTO specialOffer, List<DishEntity> mainDishes, DishEntity extraDish)
        {
            var result = new OperationResult(false);
            if(specialOffer.Discount != 0)
            {
                result.Message = $"У акции типа {specialOffer.TypeOffer} не должно быть скидки";
                return result;
            }
            if (specialOffer.MainDishes.Count == 0 || mainDishes.Count != specialOffer.MainDishes.Count)
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
