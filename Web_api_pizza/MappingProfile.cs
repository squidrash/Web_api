using System;
using AutoMapper;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerEntity, CustomerDTO>();
            CreateMap<CustomerDTO, CustomerEntity>();

            CreateMap<CustomerEntity, PersonDTO>();
            CreateMap<PersonDTO, CustomerEntity>();

            CreateMap<OrderEntity, OrderDTO>();
            CreateMap<OrderDTO, OrderEntity>();

            CreateMap<AddressEntity, AddressDTO>();
            CreateMap<AddressDTO, AddressEntity>();

            CreateMap<DishEntity, DishDTO>();
            CreateMap<DishDTO, DishEntity>();
        }
    }
}
