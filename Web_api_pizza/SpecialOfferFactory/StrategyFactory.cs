using System;
using Web_api_pizza.SpecialOfferStrategy;

namespace Web_api_pizza.SpecialOfferFactory
{
    public abstract class StrategyFactory

    {
        public abstract IComplianceSpecialOfferStrategy FactotyMethod();

        public ComplianceContext CreateStrategy()
        {
            var context = new ComplianceContext();
            var product = FactotyMethod();
            context.SetStrategy(product);
            return context;
            
        }

    }
}
