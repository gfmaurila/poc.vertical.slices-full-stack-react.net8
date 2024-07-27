using API.Admin.Domain.User;
using API.Admin.Feature.Users.GetUser;
using API.Admin.Infrastructure.Database;
using API.Admin.Tests.Integration.Utilities;
using API.Admin.Tests.Integration.Utilities.Redis;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Extensions;
using poc.core.api.net8.Interface;
using poc.core.api.net8.User;
using poc.core.api.net8.ValueObjects;

namespace API.Admin.Tests.Integration.Features.Users.Data;

public static class UserRepo
{
    public static async Task CreateUser(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        for (int i = 0; i < 10; i++)
        {
            await dbContext.User.AddAsync(InsertExistingData());
        }
        await dbContext.SaveChangesAsync();
    }

    public static async Task PopulateTestExistingData(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.User.AddAsync(InsertExistingData());
        await dbContext.SaveChangesAsync();
    }

    public static async Task<Guid> GetUserById(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var user = await dbContext.User.AddAsync(CreateUser("testedelete@teste.com.br", "Test123$"));
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static UserEntity CreateUser(string email, string password)
    {
        var faker = new Faker("pt_BR");

        var phone = new PhoneNumber(faker.Person.Phone);

        var newUser = new UserEntity(
            faker.Person.FirstName,
            faker.Person.LastName,
            EGender.Male,
            ENotificationType.WhatsApp,
            new Email(email),
            phone,
            Password.ComputeSha256Hash(password),
            RoleUserAuth(),
            new DateTime(1990, 1, 1));

        return newUser;
    }

    public static UserEntity InsertExistingData()
    {
        var faker = new Faker("pt_BR");

        var email = new Email("fulanodetal@teste.com.br");
        var phone = new PhoneNumber(faker.Person.Phone);

        var newUser = new UserEntity(
            faker.Person.FirstName,
            faker.Person.LastName,
            EGender.Male,
            ENotificationType.WhatsApp,
            email,
            phone,
            Password.ComputeSha256Hash("@G21r03a1985"),
            RoleUserAuth(),
            new DateTime(1990, 1, 1));

        return newUser;
    }

    public static async Task ClearDatabaseAsync(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var lista = await dbContext.User.ToListAsync();

        dbContext.User.RemoveRange(lista);
        await dbContext.SaveChangesAsync();

        // Limpa o Redis
        const string cacheKey = nameof(GetUserQuery);

        // Obter a instância do cache service
        var cacheService = provider.GetService<IRedisCacheService<List<UserQueryModel>>>();

        if (cacheService != null)
        {
            var redisDb = new UserTestRedisService(cacheService);

            if (lista.Any())
            {
                await redisDb.Delete(cacheKey);
            }

            await redisDb.ClearDatabaseAsync();
        }
    }

    public static List<string> RoleUserAuth()
    => new List<string>
        {
            ERoleUserAuth.USER.ToString(),
            ERoleUserAuth.CREATE_USER.ToString(),
            ERoleUserAuth.UPDATE_USER.ToString(),
            ERoleUserAuth.DELETE_USER.ToString(),
            ERoleUserAuth.GET_USER.ToString(),
            ERoleUserAuth.GET_BY_ID_USER.ToString(),

            ERoleUserAuth.NOTIFICATION.ToString(),
            ERoleUserAuth.CREATE_NOTIFICATION.ToString(),
            ERoleUserAuth.DELETE_NOTIFICATION.ToString(),
            ERoleUserAuth.GET_NOTIFICATION.ToString(),

            ERoleUserAuth.REGION.ToString(),
            ERoleUserAuth.COUNTRI.ToString(),
            ERoleUserAuth.DEPARTMENT.ToString(),
            ERoleUserAuth.EMPLOYEE.ToString(),
            ERoleUserAuth.JOB.ToString(),
            ERoleUserAuth.JOB_HISTORY.ToString(),
            ERoleUserAuth.LOCATION.ToString(),

            ERoleUserAuth.MKT_POST.ToString(),
        };
}