using System;
using Web_api_pizza.SpecialOfferStrategy;

namespace Web_api_pizza.SpecialOfferFactory
{
    public class ThreeForPriceTwoCreator : StrategyFactory
    {
       
        public override IComplianceSpecialOfferStrategy FactotyMethod()
        {
            return new ThreeForPriceTwoStrategy();
        }
    }
}
