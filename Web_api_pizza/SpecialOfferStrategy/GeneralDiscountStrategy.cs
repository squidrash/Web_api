using System;
using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class GeneralDiscountStrategy : IComplianceSpecialOfferStrategy
    {
        public bool CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var orderAmount = dishes.Sum(x => x.Price * x.Quantity);
            if(orderAmount < specialOffer.MinOrderAmount)
            {
                return false;
            }
            return true;
        }
    }
}
