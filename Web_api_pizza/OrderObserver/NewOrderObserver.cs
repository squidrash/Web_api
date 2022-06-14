using System;
using System.Threading;
using Web_api_pizza.Storage;

namespace Web_api_pizza.OrderObserver
{
    public class NewOrderObserver : IOrderObserver, INotification
    {
        private readonly ISubject _subject;
        private bool hasNewOrder;

        public NewOrderObserver(ISubject subject)
        {
            _subject = subject;
            _subject.Attach(this);
        }

        public void Update()
        {
            hasNewOrder = true;
        }

        public OperationResult CheckNewOrders()
        {
            if (hasNewOrder == false)
            {
                Thread.Sleep(2000);
                this.CheckNewOrders();
            }

            hasNewOrder = false;
            return new OperationResult(true, "Новый заказ");
        }

    }
}
