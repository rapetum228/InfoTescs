using Api.Mapper;
using Api.Services;
using Api;
using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InfotecsDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"), sql => { }); //указали имя строки подключения из appsettings.json
}, contextLifetime: ServiceLifetime.Scoped);

builder.Services.AddScoped<ValueService>();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
{
    if (serviceScope != null)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<InfotecsDataContext>();
        context.Database.Migrate();
    }
}

app.UseMiddleware<GlobalExсeptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();