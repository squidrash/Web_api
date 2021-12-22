using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Filters
{
    public class DishFilter
    {
        [FromQuery(Name = "IsActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "Category")]
        public DishCategoryEnum? Category { get; set; }

        public IQueryable<DishEntity> Filters (IQueryable<DishEntity> query)
        {
            if(IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == IsActive.Value);
            }
            if (Category.HasValue)
            {
                query = query.Where(x => x.Category == Category.Value);
            }

            return query;
        }
    }
}
