using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.Models
{
    // нужно ли добавить ссылку на заказ без пользователя?
    public class AddressEntity
    {
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string NumberOfBuild { get; set; }
        public int? NumberOfEntrance { get; set; }
        public int? Apartment { get; set; }

        public List<CustomerAddressEntity> Customers { get; set; }

        public AddressOrderEntity AddressOrder { get; set; }
    }
}
