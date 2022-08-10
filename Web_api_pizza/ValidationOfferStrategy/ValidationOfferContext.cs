using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;
using Web_api_pizza.ValidateOfferStrategy;

namespace Web_api_pizza.ValidationOfferStrategy
{
    public class ValidationOfferContext
    {
        private ValidationOfferContext(){}

        private static ValidationOfferContext _instance;

        public static ValidationOfferContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ValidationOfferContext();
            }
            return _instance;
        }

        private IValidationOfferStrategy _strategy;

        public void SetStrategy(IValidationOfferStrategy strategy)
        {
            _strategy = strategy;
        }

        public OperationResult ValidateOffer(SpecialOfferDTO specialOffer, DishEntity mainDish, DishEntity extraDish)
        {
            return _strategy.ValidateOffer(specialOffer, mainDish, extraDish);
        }
    }
}
