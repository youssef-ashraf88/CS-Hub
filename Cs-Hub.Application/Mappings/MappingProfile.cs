using AutoMapper;
using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfile, MyProfileDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<UserProfile, PublicProfileDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
