﻿using AutoMapper;
using Park.API.DTOs;
using Park.Core.Models;

namespace Park.API.Helpers
{
    public class AutoMapperPark : Profile
    {
        public AutoMapperPark()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
