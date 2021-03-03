using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Укажите имя пользователя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите фамилию пользователя")]
        public string LastName { get; set; }

        // место для списка адресов

        [Required]
        [Phone(ErrorMessage ="Укажите телефон пользователя")]
        public string Phone { get; set; }
        public int? Discount { get; set; } 

        //место для списка заказов
        public List<OrderDTO> Orders { get; set; }

    }
}
