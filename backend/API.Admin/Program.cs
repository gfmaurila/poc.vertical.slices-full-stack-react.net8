using API.Admin.Extensions;
using API.Admin.Infrastructure.Database;
using API.Admin.Infrastructure.Database.Repositories;
using API.Admin.Infrastructure.Database.Repositories.Interfaces;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using poc.core.api.net8.DistributedCache;
using poc.vertical.slices.net8.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Swagger
builder.Services.AddControllers();
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);
builder.Services.UseAuthentication(builder.Configuration);

builder.Services.AddDbContext<EFSqlServerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

DistributedCacheInitializer.Initialize(builder.Services, builder.Configuration);

builder.Services.AddTransient<IUserRepository, UserRepository>();
//builder.Services.Configure<CacheOptions1>(builder.Configuration.GetSection("CacheOptions"));

//builder.Services.AddSingleton<IConfiguration>(provider => builder.Configuration);
//builder.Services.AddSingleton<RedisConnection>();

//builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("CacheConnection")));
//builder.Services.AddScoped(typeof(IRedisCacheService<>), typeof(RedisCacheService<>));

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddAuthorization();

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});


var app = builder.Build();

if (app.Environment.IsEnvironment("Test") ||
    app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Docker") ||
    app.Environment.IsStaging() ||
    app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapCarter();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();


await app.MigrateAsync(); // Aqui faz migrations

app.Run();

public partial class Program { }
