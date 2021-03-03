using System;
using System.Collections.Generic;
using Web_api_pizza.Storage.Enums;

namespace CreateDb.Storage.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        //public int? CustomerEntityId { get; set; }
        public CustomerDTO Client { get; set; }
        public DateTime CreatTime { get; set; }
        public StatusEnum Status { get; set; }

        public AddressDTO Address { get; set; }
        public List<DishDTO> Dishes { get; set; }
    }
    

}
