using System;
using Web_api_pizza.SpecialOfferStrategy;

namespace Web_api_pizza.SpecialOfferFactory
{
    public abstract class StrategyFactory

    {
        public abstract IComplianceSpecialOfferStrategy FactotyMethod();

        public ComplianceContext CreateStrategy()
        {
            var strategyContext = ComplianceContext.GetInstance();
            var product = FactotyMethod();
            strategyContext.SetStrategy(product);
            return strategyContext;
            
        }

    }
}
