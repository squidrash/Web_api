using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class DishCategoryDTO
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
