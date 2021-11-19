using System;
using System.Collections.Generic;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.Models
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public List<OrderDishEntity> Products { get; set; }

        public DateTime CreatTime { get; set; }
        public StatusEnum Status { get; set; }
        public decimal TotalSum { get; set; }

        public AddressOrderEntity AddressOrder { get; set; }
        public CustomerOrderEntity Customer { get; set; }
    }
}
