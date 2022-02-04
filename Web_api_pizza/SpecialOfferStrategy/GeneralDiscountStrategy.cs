using System;
using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class GeneralDiscountStrategy : IComplianceSpecialOfferStrategy
    {
        public decimal CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var orderSum = dishes.Sum(x => x.Price * x.Quantity);
            if(orderSum < specialOffer.MinOrderAmount)
            {
                return -1;
            }

            var discountSum = orderSum * specialOffer.Discount / 100;
            return discountSum;
        }
    }
}
