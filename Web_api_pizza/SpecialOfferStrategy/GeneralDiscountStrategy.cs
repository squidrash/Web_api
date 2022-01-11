using System.Collections.Generic;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class GeneralDiscountStrategy : IComplianceSpecialOfferStrategy
    {
        public bool CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            return true;
        }
    }
}
