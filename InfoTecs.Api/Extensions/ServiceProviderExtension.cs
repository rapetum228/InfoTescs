using InfoTecs.Api.Services;
using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Mappers;
using InfoTecs.BLL.Services;
using InfoTecs.DAL;
using InfoTecs.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InfoTecs.Api.Extensions;

public static class ServiceProviderExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IFileProcessinger, FileProcessinger>();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        services.AddScoped<IValueHelperService, ValueHelper>();
        services.AddScoped<IResultHelperService, ResultHelper>();
        services.AddScoped<IResultRepository, ResultRepository>();
    }

    public static void AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = "MSSql";
        var projectForMigrations = "InfoTecs.Api";

        services.AddDbContext<InfotecsDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(connectionString), b => b.MigrationsAssembly(projectForMigrations));
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

