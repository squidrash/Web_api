using System;
using System.ComponentModel.DataAnnotations;
using CreateDb.Storage.Models;

namespace CreateDb.Storage.DTO
{
    public class DishDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Укажите название блюда")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Укажите цену блюда")]
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
