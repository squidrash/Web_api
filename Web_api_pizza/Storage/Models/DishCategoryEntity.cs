using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.Models
{
    public class DishCategoryEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<DishEntity> Dishes { get; set; }
    }
}
