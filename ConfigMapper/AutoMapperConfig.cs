﻿using AutoMapper;
using FoodYeah.Commons;
using FoodYeah.Dto;
using FoodYeah.Model;
using FoodYeah.Model.Identity;
using System.Linq;

namespace FoodYeah.ConfigMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<DataCollection<Customer>, DataCollection<CustomerDto>>();

            CreateMap<Product, ProductDto>();
            CreateMap<DataCollection<Product>, DataCollection<ProductDto>>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<DataCollection<Order>, DataCollection<OrderDto>>();

            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderDetailCreateDto, OrderDetail>();

            CreateMap<Card, CardDto>();
            CreateMap<DataCollection<Card>, DataCollection<CardDto>>();

            CreateMap<Product_Category, Product_CategoryDto>();
            CreateMap<DataCollection<Product_Category>, DataCollection<Product_CategoryDto>>();

            CreateMap<Customer_Category, Customer_CategoryDto>();
            CreateMap<DataCollection<Customer_Category>, DataCollection<Customer_CategoryDto>>();

            CreateMap<LOC, LOCDto>();
            CreateMap<DataCollection<LOC>, DataCollection<LOCDto>>();

            CreateMap<Transaction, TransactionDto>();
            CreateMap<DataCollection<Transaction>, DataCollection<TransactionDto>>();

            CreateMap<QuoteDetail, QuoteDetailsDto>();
            CreateMap<DataCollection<QuoteDetail>, DataCollection<QuoteDetailsDto>>();

            CreateMap<Quote, QuoteDto>();
            CreateMap<DataCollection<Quote>, DataCollection<QuoteDto>>();


            //Simple Dtos
            CreateMap<Customer, CustomerSimpleDto>();
            CreateMap<DataCollection<Customer>, DataCollection<CustomerSimpleDto>>();

            CreateMap<Transaction, TransactionSimpleDto>();
            CreateMap<DataCollection<Transaction>, DataCollection<TransactionSimpleDto>>();

            CreateMap<QuoteDetail, SimpleQuoteDetailsDtos>();
            CreateMap<DataCollection<QuoteDetail>, DataCollection<SimpleQuoteDetailsDtos>>();
            

            CreateMap<Product, ProductSimpleDto>();
            CreateMap<DataCollection<Product>, DataCollection<ProductSimpleDto>>();

            CreateMap<Order, OrderSimpleDto>();
            CreateMap<DataCollection<Order>, DataCollection<OrderSimpleDto>>();

            CreateMap<Card, CardSimpleDto>();
            CreateMap<DataCollection<Card>, DataCollection<CardSimpleDto>>();

            CreateMap<Product_Category, Product_CategorySimpleDto>();
            CreateMap<DataCollection<Product_Category>, DataCollection<Product_CategorySimpleDto>>();

            CreateMap<Customer_Category, Customer_CategorySimpleDto>();
            CreateMap<DataCollection<Customer_Category>, DataCollection<Customer_CategorySimpleDto>>();

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(
                    dest => dest.FullName,
                    opts => opts.MapFrom(src => src.LastName + ", " + src.FirstName)
                ).ForMember(
                    dest => dest.Roles,
                    opts => opts.MapFrom(src => src.UserRoles.Select(y => y.Role.Name).ToList())
                );
            CreateMap<DataCollection<ApplicationUser>, DataCollection<ApplicationUserDto>>();
        }
    }
}
