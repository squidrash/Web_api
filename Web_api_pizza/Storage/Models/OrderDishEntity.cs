using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.Models
{
    public class OrderDishEntity
    {
        public int Id { get; set; }

        public int OrderEntityId { get; set; }
        public OrderEntity Order { get; set; }

        public int DishEntityId { get; set; }
        public DishEntity Dish { get; set; }

        public int Quantity { get; set; }
    }
}
