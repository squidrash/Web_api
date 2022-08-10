using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.ValidationOfferStrategy.TemplateMethod
{
    public class ValidationExtraDishes : ValidationOfferWithDishes
    {
        protected override OperationResult ValidateOffersWithDishes(SpecialOfferDTO specialOffer, DishEntity extraDish)
        {
            var result = new OperationResult(false);
            if (specialOffer.ExtraDish == null || extraDish == null)
            {
                result.Message = "Дополнительное блюдо не соответствует блюдам из БД";
                return result;
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
