using System;
namespace Web_api_pizza.Storage.Models
{
    public class SpecialOfferOrderEntity
    {
        public int Id { get; set; }

        public int OrderEntityId { get; set; }
        public OrderEntity Order { get; set; }

        public int SpecialOfferEntity { get; set; }
        public SpecialOfferEntity SpecialOffer { get; set; }
    }
}
