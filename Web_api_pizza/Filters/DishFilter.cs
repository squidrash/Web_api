using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Filters
{
    public class DishFilter
    {
        [FromQuery(Name ="Category")]
        public DishCategoryEnum? Category { get; set; }

        public IQueryable<DishEntity> Filters (IQueryable<DishEntity> query)
        {
            if (Category.HasValue)
            {
                var dishCategory = Category.Value;
                query = query.Where(x => x.Category == dishCategory);
            }

            return query;
        }
    }
}
