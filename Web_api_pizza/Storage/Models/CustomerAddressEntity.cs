using System;
namespace Web_api_pizza.Storage.Models
{
    public class CustomerAddressEntity
    {
        public int Id { get; set; }

        public int CustomerEntityId { get; set; }
        public CustomerEntity Customer { get; set; }

        public int AddressEntityId { get; set; }
        public AddressEntity Address { get; set; }
    }
}
