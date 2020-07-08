using AutoMapper;
using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
        }
    }
}
