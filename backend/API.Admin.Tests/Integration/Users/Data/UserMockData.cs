using API.Admin.Domain.User;
using API.Admin.Feature.Auth.Login;
using API.Admin.Feature.Users.GetUser;
using API.Admin.Infrastructure.Database;
using API.Admin.Tests.Integration.Features.Auth.Fakes;
using API.Admin.Tests.Integration.Users.Fakes;
using API.Admin.Tests.Integration.Utilities.Redis;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Extensions;
using poc.core.api.net8.Interface;
using poc.core.api.net8.Response;
using poc.core.api.net8.User;
using poc.core.api.net8.ValueObjects;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Users.Data;

public class UserMockData
{
    public static async Task CreateUser(ApiUserIntegrationFactory<Program> factory, bool create)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (create)
        {
            for (int i = 0; i < 10; i++)
            {
                await dbContext.User.AddAsync(UserFake.Insert());
            }
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task<Guid> CreateUser(ApiUserIntegrationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var user = await dbContext.User.AddAsync(UserFake.Insert());
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static async Task<Guid> CreateUser(ApiUserIntegrationFactory<Program> factory, string email)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var emailL = new Email(email);

        var user = await dbContext.User.AddAsync(UserFake.Insert(emailL));
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static async Task DeleteUser(ApiUserIntegrationFactory<Program> factory, bool delete)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (delete)
        {
            var lista = dbContext.User.ToList();

            foreach (var item in lista)
            {
                dbContext.User.Remove(item);
                await dbContext.SaveChangesAsync();
            }
        }

        // Limpa o redis
        const string cacheKey = nameof(GetUserQuery);

        // Obter a instância do cache service
        var cacheService = provider.GetService<IRedisCacheService<List<UserQueryModel>>>();

        // Instanciar UserTestRedisService com a instância do cache service
        var redisDb = new UserTestRedisService(cacheService);

        // Usar o serviço
        await redisDb.Delete(cacheKey);
    }

    public static async Task CreateUserExistingData(ApiUserIntegrationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.User.AddAsync(UserFake.InsertExistingData());
        await dbContext.SaveChangesAsync();
    }

    #region Auth
    public static async Task<ApiResult<AuthTokenResponse>> GetAuthAsync(ApiUserIntegrationFactory<Program> factory, HttpClient client)
    {
        var command = AuthFake.GetAuthAsync();
        await GetAuth(factory);
        var httpResponse = await client.PostAsJsonAsync("/api/v1/login", command);
        return await httpResponse.Content.ReadFromJsonAsync<ApiResult<AuthTokenResponse>>();
    }

    public static async Task<Guid> GetAuth(ApiUserIntegrationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var user = await dbContext.User.AddAsync(CreateUserAuth("auth@auth.com.br", "Test123$"));
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static UserEntity CreateUserAuth(string email, string password)
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

    public static async Task ClearDatabaseAsync(ApiUserIntegrationFactory<Program> factory)
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
    #endregion
}

