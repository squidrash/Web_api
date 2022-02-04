using System;
using System.Collections.Generic;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public class ComplianceContext
    {
        private IComplianceSpecialOfferStrategy _strategy;

        public ComplianceContext()
        { }

        public ComplianceContext(IComplianceSpecialOfferStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IComplianceSpecialOfferStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal DoSomeBusinessLogic(List<DishDTO> dishes, SpecialOfferEntity offer)
        {
            var result = _strategy.CheckComplianceSpecialOffer(dishes, offer);
            return result;
        }
    }
}
