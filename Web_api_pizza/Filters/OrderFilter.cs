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
        public StatusEnum? Status { get; set; }

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
                    query = query.Where(x => x.CustomerEntityId != null);
                }
                else
                {
                    query = query.Where(x => x.CustomerEntityId == null);
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
            if(Status.HasValue)
            {
                switch(Status.Value)
                {
                    case StatusEnum.New:
                        query = query.Where(x => x.Status == StatusEnum.New);
                        break;
                    case StatusEnum.Confirmed:
                        query = query.Where(x => x.Status == StatusEnum.Confirmed);
                        break;
                    case StatusEnum.Preparing:
                        query = query.Where(x => x.Status == StatusEnum.Preparing);
                        break;
                    case StatusEnum.OnTheWay:
                        query = query.Where(x => x.Status == StatusEnum.OnTheWay);
                        break;
                    case StatusEnum.Delivered:
                        query = query.Where(x => x.Status == StatusEnum.Delivered);
                        break;
                    case StatusEnum.Cancelled:
                        query = query.Where(x => x.Status == StatusEnum.Cancelled);
                        break;
                    default:
                        break;
                }
                    
            }
                
            return query;
        }

        
    }
}
