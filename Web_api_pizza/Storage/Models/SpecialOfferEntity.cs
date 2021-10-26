using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.Models
{
    public class SpecialOfferEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        // добавить типы акций 
        //[Required]
        //public TypeOfferEnum TypeOffer { get; set; }

        //добавить число блюд необходимо для акции

        // добавить чисто доп блюд по акции

        public int Discount { get; set; }

        //public List<> Products { get; set; }
    }
}
