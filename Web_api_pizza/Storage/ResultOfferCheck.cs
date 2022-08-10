using System;
namespace Web_api_pizza.Storage
{
    public class ResultOfferCheck : OperationResult
    {
        public decimal DiscountSum { get; set; }
        public ResultOfferCheck(bool isSuccess, string message, decimal discountSum) : base(isSuccess, message)
        {
            DiscountSum = discountSum;
        }
    }
}
