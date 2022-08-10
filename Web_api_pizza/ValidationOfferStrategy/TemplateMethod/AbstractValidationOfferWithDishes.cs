using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;
using Web_api_pizza.ValidateOfferStrategy;

namespace Web_api_pizza.ValidationOfferStrategy
{
    public abstract class ValidationOfferWithDishes : IValidationOfferStrategy
    {
        public OperationResult ValidateOffer(SpecialOfferDTO specialOffer, DishEntity mainDish, DishEntity extraDish)
        {
            var result = new OperationResult(false);
            if (specialOffer.Discount != 0)
            {
                result.Message = $"Для акции типа {specialOffer.TypeOffer} не должно быть скидки";
                return result;
            }

            if (specialOffer.MinOrderAmount != 0)
            {
                result.Message = $"Для акции типа {specialOffer.TypeOffer} не должно быть указана минимальная сумма";
                return result;
            }

            if (specialOffer.MainDish == null || mainDish == null)
            {
                result.Message = "Список основных блюд не соответствует блюдам из БД";
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


            result = ValidateOffersWithDishes(specialOffer, extraDish);

            if (result.IsSuccess == false)
            {
                return result;
            }

            result.Message = "Успешно";
            return result;
        }

        protected abstract OperationResult ValidateOffersWithDishes(SpecialOfferDTO specialOffer, DishEntity extraDish);

        
    }
}
