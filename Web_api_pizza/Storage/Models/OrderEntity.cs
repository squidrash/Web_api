using System;
using System.Collections.Generic;

namespace CreateDb.Storage.Models
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public int? CustomerEntityId { get; set; }
        public CustomerEntity Customer { get; set; }

        public List<OrderDishEntity> Products { get; set; }

        public DateTime CreatTime { get; set; } 
        public Status Status { get; set; }

        public AddressOrderEntity AddressOrder { get; set; }
    }
    public enum Status
    {
        New = 1,
        Preparing,
        OnTheWay,
        Delivered,
        Cancelled
    }
}
