using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;
using Web_api_pizza.ValidateOfferStrategy;

namespace Web_api_pizza.ValidationOfferStrategy.Adapter
{
    public class ValidationStrategyAdapter :IValidationOfferStrategy
    {
        private readonly ValidationGeneralDiscount _validation;
        public ValidationStrategyAdapter(ValidationGeneralDiscount validation)
        {
            _validation = validation;
        }

        public OperationResult ValidateOffer(SpecialOfferDTO specialOffer, DishEntity mainDish, DishEntity extraDish)
        {
            return _validation.ValidateGeneralDiscount(specialOffer);
        }
    }
}
