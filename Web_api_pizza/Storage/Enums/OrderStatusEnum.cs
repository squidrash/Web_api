using System;
namespace Web_api_pizza.Storage.Enums
{
    public enum OrderStatusEnum
    {
        New = 1,
        Confirmed,
        Preparing,
        OnTheWay,
        Delivered,
        Cancelled
    }
}
