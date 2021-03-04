using System;
using System.Collections.Generic;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Storage.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        //public int? CustomerEntityId { get; set; }
        public PersonDTO Client { get; set; }
        public DateTime CreatTime { get; set; }
        public StatusEnum Status { get; set; }

        public AddressDTO Address { get; set; }
        public List<DishDTO> Dishes { get; set; }
    }
    

}
