using InfoTecs.Api.Helpers;
using Api.Mapper;
using InfoTecs.Api.Services;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using SystemInterface.IO;
using SystemWrapper.IO;
using Api.Helpers;

namespace Api.Infastructures;

public static class ServiceProviderExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IValueService, ValueService>();
        services.AddScoped<IFileProcessingService, FileProcessingService>();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        services.AddScoped<IValueHelperService, ValueHelper>();
        services.AddScoped<IResultHelperService, ResultHelper>();
        services.AddScoped<IFile, FileWrap>();
    }

    public static void AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<InfotecsDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MSSql"), sql => { });
        }, contextLifetime: ServiceLifetime.Scoped);
    }

    public static void RegisterSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.EnableAnnotations();
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //config.IncludeXmlComments(xmlPath);
        });
    }


}

