﻿using System;
namespace Web_api_pizza.Storage.Models
{
    public class AddressOrderEntity
    {
        public int Id { get; set; }

        public int OrderEntityId { get; set; }
        public OrderEntity Order { get; set; }

        public int AddressEntityId { get; set; }
        public AddressEntity Address { get; set; }
    }
}
