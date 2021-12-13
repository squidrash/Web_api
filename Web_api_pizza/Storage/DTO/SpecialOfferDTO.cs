using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.DTO
{
    public class SpecialOfferDTO
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Description { get; set; }

        //нужно сделать уникальным
        [Required]
        public string PromoCode { get; set; }

        //тип акции
        //для каждой акции свой подсчет
        [Required]
        public TypeOfferEnum TypeOffer { get; set; }

        //только для акций типа общая скидка
        public int Discount { get; set; }

        //блюда участвующие в акции
        public List<DishDTO> MainDishes { get; set; }

        //число блюд необходимо для акций типа 1+1=3 и блюдо в подарок
        public int RequiredNumberOfDish;

        //число доп блюд по акции "блюдо в подарок"
        public int NumberOfExtraDish;

        
        //доп блюдо
        public DishDTO ExtraDish { get; set; }
    }
}
