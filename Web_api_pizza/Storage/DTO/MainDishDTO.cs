using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class MainDishDTO
    {
        public int? Id { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Укажите название блюда")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Укажите цену блюда")]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }
    }
}
