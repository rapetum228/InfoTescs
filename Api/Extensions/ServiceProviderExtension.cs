using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Mappers;
using InfoTecs.Api.Services;
using InfoTecs.DAL;
using Microsoft.EntityFrameworkCore;
using InfoTecs.DAL.Repositories;
using InfoTecs.BLL.Services;

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
        services.AddScoped<IResultRepository, ResultRepository>();
    }

    public static void AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<InfotecsDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MSSql"), b => b.MigrationsAssembly("InfoTecs.Api"));
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

