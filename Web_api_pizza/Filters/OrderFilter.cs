using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Filters
{
    public class OrderFilter
    {
        //[FromQuery(Name = "customer_id")]
        //public int? CustomerId { get; set; }

        [FromQuery(Name="has_customer")]
        public bool? HasCustomer { get; set; }

        [FromQuery(Name = "has_address")]
        public bool? HasAddress { get; set; }

        [FromQuery(Name = "status")]
        public OrderStatusEnum? OrderStatus { get; set; }

        public IQueryable<OrderEntity> Filters(IQueryable<OrderEntity> query)
        {
            //if(CustomerId.HasValue)
            //{
            //    query = query.Where(x => x.CustomerEntityId == CustomerId);
            //}
            if (HasCustomer.HasValue)
            {
                if (HasCustomer == true)
                {
                    query = query.Where(x => x.Customer != null);
                }
                else
                {
                    query = query.Where(x => x.Customer == null);
                }
            }
            if (HasAddress.HasValue)
            {
                if(HasAddress == true)
                {
                    query = query.Where(x => x.AddressOrder != null);
                }
                else
                {
                    query = query.Where(x => x.AddressOrder == null);
                }
            }
            if(OrderStatus.HasValue)
            {
                var status = OrderStatus.Value;
                query = query.Where(x => x.Status == status);
            }
                
            return query;
        }

        
    }
}
