using AutoMapper;
using InfoTecs.BLL.Models;
using InfoTecs.DAL.Additions;
using InfoTecs.DAL.Entities;

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

