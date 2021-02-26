using System;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumberOfBuild { get; set; }
        public int? NumberOfEntrance { get; set; }
        public int? Apartment { get; set; }
    }
}
