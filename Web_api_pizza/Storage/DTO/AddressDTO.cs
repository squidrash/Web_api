using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название города")]
        public string City { get; set; }
        [Required(ErrorMessage = "Введите название улицы")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Введите номер дома")]
        public string NumberOfBuild { get; set; }
        public int? NumberOfEntrance { get; set; }
        public int? Apartment { get; set; }
    }
}
