using AutoMapper;
using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO.Profiles
{
    public class ApplicationRoleProfile : Profile
    {
        public ApplicationRoleProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ReverseMap();
        }
    }
}
