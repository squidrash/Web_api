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
                .ForMember(dto => dto.Dishes, opt => opt.MapFrom(en => en.Products))
                .ForMember(dto => dto.Client, opt => opt.MapFrom(en => en.Customer));
            // - Order
            CreateMap<OrderDishEntity, DishDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Dish.Id))
                .ForMember(dto => dto.ProductName, opt => opt.MapFrom(en => en.Dish.ProductName))
                .ForMember(dto => dto.Price, opt => opt.MapFrom(en => en.Dish.Price))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(en => en.Quantity))
                .ForMember(dto => dto.Category, opt => opt.MapFrom(en => en.Dish.Category))
                .ForMember(dto => dto.IsActive, opt => opt.MapFrom(en => en.Dish.IsActive))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(en => en.Dish.Description))
                .ForMember(dto => dto.Image, opt => opt.MapFrom(en => en.Dish.Image));
            CreateMap<AddressOrderEntity, AddressDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Address.Id))
                .ForMember(dto => dto.City, opt => opt.MapFrom(en => en.Address.City))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(en => en.Address.Street))
                .ForMember(dto => dto.NumberOfBuild, opt => opt.MapFrom(en => en.Address.NumberOfBuild))
                .ForMember(dto => dto.NumberOfEntrance, opt => opt.MapFrom(en => en.Address.NumberOfEntrance))
                .ForMember(dto => dto.Apartment, opt => opt.MapFrom(en => en.Address.Apartment));

            CreateMap<CustomerOrderEntity, CustomerDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Customer.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(en => en.Customer.Name))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(en => en.Customer.LastName))
                .ForMember(dto => dto.Phone, opt => opt.MapFrom(en => en.Customer.Phone))
                .ForMember(dto => dto.Discount, opt => opt.MapFrom(en => en.Customer.Discount));

            CreateMap<CustomerOrderEntity, OrderDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Order.Id))
                .ForMember(dto => dto.Status, opt => opt.MapFrom(en => en.Order.Status))
                .ForMember(dto => dto.CreateTime, opt => opt.MapFrom(en => en.Order.CreateTime))
                .ForMember(dto => dto.TotalSum, opt => opt.MapFrom(en => en.Order.TotalSum))
                .ForMember(dto => dto.Dishes, opt => opt.MapFrom(en => en.Order.Products))
                .ForMember(dto => dto.Address, opt => opt.MapFrom(en => en.Order.AddressOrder))
                .ForMember(dto => dto.DiscountSum, opt => opt.MapFrom(en => en.Order.DiscountSum));



            CreateMap<CustomerAddressEntity, AddressDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Address.Id))
                .ForMember(dto => dto.City, opt => opt.MapFrom(en => en.Address.City))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(en => en.Address.Street))
                .ForMember(dto => dto.NumberOfBuild, opt => opt.MapFrom(en => en.Address.NumberOfBuild))
                .ForMember(dto => dto.NumberOfEntrance, opt => opt.MapFrom(en => en.Address.NumberOfEntrance))
                .ForMember(dto => dto.Apartment, opt => opt.MapFrom(en => en.Address.Apartment));

            CreateMap<CustomerAddressEntity, CustomerDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(en => en.Customer.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(en => en.Customer.Name))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(en => en.Customer.LastName))
                .ForMember(dto => dto.Phone, opt => opt.MapFrom(en => en.Customer.Phone))
                .ForMember(dto => dto.Discount, opt => opt.MapFrom(en => en.Customer.Discount));




            //
            CreateMap<OrderDTO, OrderEntity>();
                //.ForMember(x => x.AddressOrder.Address, opt => opt.MapFrom(m => m.Address));
                //.ForMember(en => en.Products, opt => opt.MapFrom(dto => dto.Dishes));

            CreateMap<AddressEntity, AddressDTO>();
            CreateMap<AddressDTO, AddressEntity>();

            CreateMap<DishEntity, DishDTO>();
            CreateMap<DishDTO, DishEntity>();
            //.ForMember(src => src.OfferMainDishes, opt => opt.Ignore())
            //.ForMember(src => src.OfferExtraDish, opt => opt.Ignore());

            CreateMap<DishCategoryEntity, MenuDTO>()
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(en => en.Id))
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(en => en.Name));
            CreateMap<DishEntity, MainDishDTO>();

            CreateMap<DishCategoryEntity, DishCategoryDTO>();
            CreateMap<DishCategoryDTO, DishCategoryEntity>();

            CreateMap<SpecialOfferEntity, SpecialOfferDTO>();
                //.ForMember(src => src.MainDishes, opt => opt.Ignore())
                //.ForMember(src => src.ExtraDish, opt => opt.Ignore());
            CreateMap<SpecialOfferDTO, SpecialOfferEntity>();

        }
    }
}
