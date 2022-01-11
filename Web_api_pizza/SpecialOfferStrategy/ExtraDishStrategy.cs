using System.Collections.Generic;
using System.Linq;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ExtraDishStrategy : IComplianceSpecialOfferStrategy
    {
        public bool CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            var checkMainDish = dishes
                        .Where(x => x.Id == specialOffer.MainDishId
                        && x.Quantity >= specialOffer.RequiredNumberOfDish)
                        .FirstOrDefault();
            var checkExtraDish = dishes
                .Where(x => x.Id == specialOffer.ExtraDishId
                && x.Quantity >= specialOffer.NumberOfExtraDish)
                .FirstOrDefault();
            if (checkMainDish == null || checkExtraDish == null)
            {
                return false;
            }
            return true;
        }
    }
}
