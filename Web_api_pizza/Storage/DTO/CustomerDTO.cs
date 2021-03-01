using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

        // место для списка адресов

        [Required]
        public string Phone { get; set; }
        public int Discount { get; set; } 

        //место для списка заказов
        public List<OrderDTO> Orders { get; set; }
    }
}
