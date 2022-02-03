using System;
using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ThreeForPriceTwoStrategy : IComplianceSpecialOfferStrategy
    {
        public bool CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var checkMainDish = dishes
                        .Where(x => x.Id == specialOffer.MainDishId
                        && x.Quantity >= specialOffer.RequiredNumberOfDish + specialOffer.NumberOfExtraDish)
                        .FirstOrDefault();
            Console.WriteLine(checkMainDish);
            if (checkMainDish == null)
            {
                return false;
            }
            return true;
        }
    }
}
