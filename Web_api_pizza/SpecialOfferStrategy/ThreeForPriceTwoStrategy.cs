using System;
using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ThreeForPriceTwoStrategy : IComplianceSpecialOfferStrategy
    {
        public decimal CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var mainDish = dishes
                        .Where(x => x.Id == specialOffer.MainDishId
                        && x.Quantity >= specialOffer.RequiredNumberOfDish + specialOffer.NumberOfExtraDish)
                        .FirstOrDefault();
            if (mainDish == null)
            {
                return -1;
            }
            var discountSum = mainDish.Price * specialOffer.NumberOfExtraDish;
            return discountSum;
        }
    }
}
