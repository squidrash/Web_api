using System;
namespace CreateDb.Storage.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int? CustomerEntityId { get; set; }
        //public CustomerEntity Customer { get; set; }

        //public List<OrderMenuEntity> Products { get; set; }

        public DateTime CreatTime { get; set; }
        public StatusDTO Status { get; set; }

        //public AddressOrderEntity AddressOrder { get; set; }
    }
    public enum StatusDTO
    {
        New = 1,
        Preparing,
        OnTheWay,
        Delivered,
        Cancelled
    }

}
