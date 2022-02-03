using System;
using System.Collections.Generic;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        
        public CustomerDTO Client { get; set; }
        public DateTime CreateTime { get; set; }
        public OrderStatusEnum Status { get; set; }
        public decimal TotalSum { get; set; }
        public decimal DiscountSum { get; set; }

        public AddressDTO Address { get; set; }
        public List<DishDTO> Dishes { get; set; }
    }
    

}
