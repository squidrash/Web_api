using System;
namespace Web_api_pizza.OrderObserver
{
    public interface ISubject
    {
        void Attach(IOrderObserver observer);

        void Detach(IOrderObserver observer);

        void Notify();
    }
}
