using System;
using System.ComponentModel.DataAnnotations;

namespace Web_api_pizza.Storage.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите фамилию пользователя")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Укажите телефон пользователя")]
        [Phone]
        public string Phone { get; set; }
        public int? Discount { get; set; }
    }
}
