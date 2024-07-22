using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using poc.admin.Extensions;
using poc.admin.Infrastructure.Database;
using poc.admin.Infrastructure.Database.Repositories;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.DistributedCache;
using poc.vertical.slices.net8.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Swagger
builder.Services.AddControllers();
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);





builder.Services.AddDbContext<EFSqlServerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

DistributedCacheInitializer.Initialize(builder.Services, builder.Configuration);

builder.Services.AddTransient<IUserRepository, UserRepository>();



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


if (app.Environment.IsEnvironment("Test") ||
    app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Docker") ||
    app.Environment.IsStaging() ||
    app.Environment.IsProduction())
{
    //await app.MigrateAsync(); // Aqui faz migrations
}

app.Run();

public partial class Program { }
