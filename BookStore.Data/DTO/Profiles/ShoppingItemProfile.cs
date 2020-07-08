using AutoMapper;
using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO.Profiles
{
    public class ShoppingItemProfile:Profile
    {
        public ShoppingItemProfile()
        {
            CreateMap<ShoppingItem, ShoppingItemDTO>()
                .ReverseMap();
        }
    }
}
