using System;
using System.ComponentModel.DataAnnotations;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.DTO
{
    public class DishDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Укажите название блюда")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Укажите цену блюда")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Укажите категорию блюда")]
        public DishCategoryEnum Category { get; set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public int Quantity { get; set; }
    }
}
