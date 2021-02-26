using System;
using AutoMapper;
using CreateDb.Storage.DTO;
using CreateDb.Storage.Models;

namespace Web_api_pizza
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerEntity, CustomerDTO>();
            CreateMap<CustomerDTO, CustomerEntity>();

            CreateMap<OrderEntity, OrderDTO>();
            CreateMap<OrderDTO, OrderEntity>();

            CreateMap<AddressEntity, AddressDTO>();
            CreateMap<AddressDTO, AddressEntity>();

            CreateMap<DishEntity, DishDTO>();
            CreateMap<DishDTO, DishEntity>();
        }
    }
}
