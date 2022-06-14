using System;
using System.Collections.Generic;

namespace Web_api_pizza.OrderObserver
{
    public class NewOrderSubject : ISubject
    {

        private List<IOrderObserver> _observers = new List<IOrderObserver>();

        public void Attach(IOrderObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach( var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
