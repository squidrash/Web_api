using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ExtraDishStrategy : IComplianceSpecialOfferStrategy
    {
        public decimal CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var mainDish = dishes
                        .Where(x => x.Id == specialOffer.MainDishId
                        && x.Quantity >= specialOffer.RequiredNumberOfDish)
                        .FirstOrDefault();
            var extraDish = dishes
                .Where(x => x.Id == specialOffer.ExtraDishId
                && x.Quantity >= specialOffer.NumberOfExtraDish)
                .FirstOrDefault();
            if (mainDish == null || extraDish == null)
            {
                return -1;
            }

            var discountSum = extraDish.Price * specialOffer.NumberOfExtraDish;
            return discountSum;
        }
    }
}
