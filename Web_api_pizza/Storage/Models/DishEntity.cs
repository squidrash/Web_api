using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.Models
{
    public class DishEntity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Укажите название блюда")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Укажите цену блюда")]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }
        //[Required(ErrorMessage = "Укажите категорию блюда")]
        [IgnoreMap]
        public DishCategoryEntity Category { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }


        public List<SpecialOfferEntity> OfferMainDishes { get; set; }
        public List<SpecialOfferEntity> OfferExtraDish { get; set; }
    }
}
