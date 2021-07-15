﻿using System;
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

            CreateMap<OrderEntity, OrderDTO>()
                .ForMember(dto => dto.Address, opt => opt.MapFrom(en => en.AddressOrder))
                .ForMember(dto => dto.Dishes, opt => opt.MapFrom(en => en.Products));
            // - Order
            CreateMap<OrderDishEntity, DishDTO>()
                .ForMember(dto => dto.ProductName, opt => opt.MapFrom(en => en.Dish.ProductName))
                .ForMember(dto => dto.ProductName, opt => opt.MapFrom(en => en.Dish.Price))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(en => en.Quantity));
            CreateMap<AddressOrderEntity, AddressDTO>()
                .ForMember(dto => dto.City, opt => opt.MapFrom(en => en.Address.City))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(en => en.Address.Street))
                .ForMember(dto => dto.NumberOfBuild, opt => opt.MapFrom(en => en.Address.NumberOfBuild))
                .ForMember(dto => dto.NumberOfEntrance, opt => opt.MapFrom(en => en.Address.NumberOfEntrance))
                .ForMember(dto => dto.Apartment, opt => opt.MapFrom(en => en.Address.Apartment));
            //
            CreateMap<OrderDTO, OrderEntity>();
                //.ForMember(x => x.AddressOrder.Address, opt => opt.MapFrom(m => m.Address));
                //.ForMember(en => en.Products, opt => opt.MapFrom(dto => dto.Dishes));

            CreateMap<AddressEntity, AddressDTO>();
            CreateMap<AddressDTO, AddressEntity>();

            CreateMap<DishEntity, DishDTO>();
            CreateMap<DishDTO, DishEntity>();
        }
    }
}
