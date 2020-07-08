using AutoMapper;
using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO.Profiles
{
    public class CompanyProfile :Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDTO>()
                .ReverseMap(); 
        }
    }
}
