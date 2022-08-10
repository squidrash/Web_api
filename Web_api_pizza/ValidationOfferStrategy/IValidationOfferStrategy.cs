using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.ValidateOfferStrategy
{
    public interface IValidationOfferStrategy
    {
        OperationResult ValidateOffer(SpecialOfferDTO specialOffer, DishEntity mainDish, DishEntity extraDish);
    }
}
