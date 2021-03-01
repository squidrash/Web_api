using System;
using Web_api_pizza.Storage.Enums;

namespace CreateDb.Storage.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int? CustomerEntityId { get; set; }
        //public CustomerEntity Customer { get; set; }

        //public List<OrderMenuEntity> Products { get; set; }

        public DateTime CreatTime { get; set; }
        public StatusEnum Status { get; set; }

        //public AddressOrderEntity AddressOrder { get; set; }
    }
    

}
