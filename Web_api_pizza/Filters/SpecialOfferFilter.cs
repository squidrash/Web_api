using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Filters
{
    public class SpecialOfferFilter
    {
        [FromQuery(Name = "TypeOffer")]
        public TypeOfferEnum? TypeOffer{ get; set; }

        public IQueryable<SpecialOfferEntity> Filters(IQueryable<SpecialOfferEntity> query)
        {
            if (TypeOffer.HasValue)
            {
                var typeOffer = TypeOffer.Value;
                query = query.Where(x => x.TypeOffer == typeOffer);
            }
            return query;
        }

    }
}
