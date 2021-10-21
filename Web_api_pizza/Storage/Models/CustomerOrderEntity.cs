using System;
namespace Web_api_pizza.Storage.Models
{
    public class CustomerOrderEntity
    {
        public int Id { get; set; }

        public int? CustomerEntityId { get; set; }
        public CustomerEntity Customer { get; set; }

        public int OrderEntityId { get; set; }
        public OrderEntity Order { get; set; }
    }
}
