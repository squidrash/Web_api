using System;
using System.Collections.Generic;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.SpecialOfferStrategy
{
    public sealed class ComplianceContext 
    {

        private ComplianceContext() { }

        private static ComplianceContext _instance;

        public static ComplianceContext GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ComplianceContext();
            }
            return _instance;
        }


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
