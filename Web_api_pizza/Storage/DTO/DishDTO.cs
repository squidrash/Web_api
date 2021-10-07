using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class DishDTO
    {
        public int? Id { get; set; }
        //[Required(ErrorMessage = "Укажите название блюда")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Укажите цену блюда")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
