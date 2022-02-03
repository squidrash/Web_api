using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class MenuDTO
    {
        public int? CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }

        public List<MainDishDTO> Dishes { get; set; }
    }
}
