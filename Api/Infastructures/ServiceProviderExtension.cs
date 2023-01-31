using Api.Mapper;
using Api.Services;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Api.Infastructures;

public static class ServiceProviderExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IValueService, ValueService>();
        services.AddScoped<IFileProcessingService, FileProcessingService>();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
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
        });
    }
}

