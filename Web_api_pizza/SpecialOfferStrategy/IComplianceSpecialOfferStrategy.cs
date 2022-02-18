using System;
using System.Collections.Generic;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public interface IComplianceSpecialOfferStrategy
    {
        public decimal CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer);
    }
}
