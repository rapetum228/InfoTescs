﻿using InfoTecs.BLL.Models;
using AutoMapper;
using InfoTecs.DAL.Entities;
using InfoTecs.DAL.Additions;

namespace InfoTecs.BLL.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ValueModel, Value>().ReverseMap();
        CreateMap<ResultModel, Result>().ReverseMap();
        CreateMap<ResultModel, ResultOutputModel>();
        CreateMap<Period, PeriodModel>().ReverseMap();
        CreateMap<ResultRequestModel, ResultRequest>();
    }
}
