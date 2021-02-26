using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.Models
{
    public class CustomerEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public List<CustomerAddressEntity> Addresses { get; set; }
        [Required]
        public string Phone { get; set; }
        public int Discount { get; set; }

        public List<OrderEntity> Orders { get; set; }
    }
}
