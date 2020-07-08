using AutoMapper;
using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO.Profiles
{
    public class OrderDetailsProfile:Profile
    {
        public OrderDetailsProfile()
        {
            CreateMap<OrderDetails, OrderDetailsDTO>()
                .ReverseMap();
        }
    }
}
