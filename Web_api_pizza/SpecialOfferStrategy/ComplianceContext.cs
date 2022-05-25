using System;
using System.Collections.Generic;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ComplianceContext : IComplianceSpecialOfferStrategy
    {
        private IComplianceSpecialOfferStrategy _strategy;

        public void SetStrategy(IComplianceSpecialOfferStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CheckComplianceSpecialOffer(List<DishDTO> dishes, SpecialOfferEntity specialOffer)
        {
            return _strategy.CheckComplianceSpecialOffer(dishes, specialOffer);
        }
    }
}
