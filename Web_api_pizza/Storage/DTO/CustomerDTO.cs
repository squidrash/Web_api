using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Storage.DTO
{
    public class CustomerDTO : PersonDTO
    {
        //место для списка заказов
        public List<OrderDTO> Orders { get; set; }
        // место для списка адресов
        public List<AddressDTO> Address { get; set; }

    }
}
