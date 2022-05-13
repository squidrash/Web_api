using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.Models
{
    public class SpecialOfferEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название Акции")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо описание предложения")]
        [StringLength(300, MinimumLength = 3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Укажите промокод для предложения")]
        public string PromoCode { get; set; }

        //тип акции
        //для каждой акции свой подсчет
        [Required(ErrorMessage = "Укажите тип предложения")]
        public TypeOfferEnum TypeOffer { get; set; }

        //только для акций типа общая скидка
        public int Discount { get; set; }

        //блюдо участвующие в акции
        public int? MainDishId { get; set; }
        [IgnoreMap]
        public DishEntity MainDish { get; set; }

        //число блюд необходимо для акций типа 1+1=3 и блюдо в подарок
        public int RequiredNumberOfDish { get; set; }

        //число доп блюд по акции "блюдо в подарок"
        public int NumberOfExtraDish { get; set; }

        //доп блюдо
        public int? ExtraDishId { get; set; }
        [IgnoreMap]
        public DishEntity ExtraDish { get; set; }

        //минимальная сумма заказа для акции типа GeneralDiscount
        public decimal MinOrderAmount { get; set; }

        public string Image { get; set; }


    }
}
