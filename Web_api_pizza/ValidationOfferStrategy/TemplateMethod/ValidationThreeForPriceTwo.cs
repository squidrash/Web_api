using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.ValidationOfferStrategy.TemplateMethod
{
    public class ValidationThreeForPriceTwo : ValidationOfferWithDishes
    {
        protected override OperationResult ValidateOffersWithDishes(SpecialOfferDTO specialOffer, DishEntity extraDish)
        {
            var result = new OperationResult(false);
            if (specialOffer.MainDish.Id != extraDish.Id)
            {
                result.Message = $"Для акции типа {specialOffer.TypeOffer} основное и доп блюдо должны быть одинаковыми";
                return result;
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
