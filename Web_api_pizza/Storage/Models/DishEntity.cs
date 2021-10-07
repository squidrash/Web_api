using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.Models
{
    public class DishEntity
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public List<OrderDishEntity> Orders { get; set; }

    }
}
