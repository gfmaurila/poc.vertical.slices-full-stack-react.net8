using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using poc.admin.Database.Repositories;
using poc.admin.Database.Repositories.Interfaces;
using poc.core.api.net8.DistributedCache;
using poc.vertical.slices.net8.Database;
using poc.vertical.slices.net8.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker") || app.Environment.IsStaging())
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
