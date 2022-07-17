using System;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Storage
{
    public class ResultOfferValidation : OperationResult
    {
        public DishEntity MainDish { get; set; }
        public DishEntity ExtraDish { get; set; }
        public ResultOfferValidation(bool isSuccess, string message, DishEntity mainDish, DishEntity extraDish) : base (isSuccess, message)
        {
            MainDish = mainDish;
            ExtraDish = extraDish;
        }
    }
}
