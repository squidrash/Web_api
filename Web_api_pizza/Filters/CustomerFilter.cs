using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Filters
{
    public class CustomerFilter
    {
        [FromQuery(Name = "Name")]
        public string Name { get; set; }

        [FromQuery(Name = "LastName")]
        public string LastName { get; set; }

        [FromQuery(Name = "Phone")]
        public string Phone { get; set; }

        public IQueryable<CustomerEntity> Filters(IQueryable<CustomerEntity> query)
        {
            if (Name != null)
            {
                query = query.Where(x => x.Name == Name);
            }
            if (LastName != null)
            {
                query = query.Where(x => x.LastName == LastName);
            }
            if (Phone != null)
            {
                query = query.Where(x => x.Phone == Phone);
            }

            return query;
        }
    }
}
