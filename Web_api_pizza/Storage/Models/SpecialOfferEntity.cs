using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.Models
{
    public class SpecialOfferEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public string PromoCode { get; set; }

        //тип акции
        //для каждой акции свой подсчет
        [Required]
        public TypeOfferEnum TypeOffer { get; set; }

        //только для акций типа общая скидка
        public int Discount { get; set; }

        //блюда участвующие в акции
        [IgnoreMap]
        public List<DishEntity> MainDishes { get; set; }

        //число блюд необходимо для акций типа 1+1=3 и блюдо в подарок
        public int RequiredNumberOfDish { get; set; }

        //число доп блюд по акции "блюдо в подарок"
        public int NumberOfExtraDish { get; set; }

        //доп блюдо
        public int? ExtraDishId { get; set; }
        [IgnoreMap]
        public DishEntity ExtraDish { get; set; }




    }
}
