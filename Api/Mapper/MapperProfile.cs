using InfoTecs.Api.Models;
using AutoMapper;
using DAL.Entities;

namespace Api.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ValueModel, Value>().ReverseMap();
        CreateMap<ResultModel, Result>().ReverseMap();
        CreateMap<ResultModel, ResultOutputModel>();
        CreateMap<Period, PeriodModel>().ReverseMap();
    }
}

