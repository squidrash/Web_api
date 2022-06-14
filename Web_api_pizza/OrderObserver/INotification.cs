using System;
using Web_api_pizza.Storage;

namespace Web_api_pizza.OrderObserver
{
    public interface INotification
    {
        OperationResult CheckNewOrders();
    }
    
}
