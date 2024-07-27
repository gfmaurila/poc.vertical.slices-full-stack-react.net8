using API.Admin.Feature.Users.GetUser;
using API.Admin.Infrastructure.Database;
using API.Admin.Tests.Integration.Users.Fakes;
using API.Admin.Tests.Integration.Utilities;
using API.Admin.Tests.Integration.Utilities.Redis;
using Microsoft.Extensions.DependencyInjection;
using poc.core.api.net8.Interface;
using poc.core.api.net8.ValueObjects;

namespace API.Admin.Tests.Integration.Users.Data;

public class UserMockData
{
    public static async Task CreateUser(CustomWebApplicationFactory<Program> factory, bool create)
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

    public static async Task<Guid> CreateUser(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var user = await dbContext.User.AddAsync(UserFake.Insert());
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static async Task<Guid> CreateUser(CustomWebApplicationFactory<Program> factory, string email)
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

    public static async Task DeleteUser(CustomWebApplicationFactory<Program> factory, bool delete)
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

    public static async Task CreateUserExistingData(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.User.AddAsync(UserFake.InsertExistingData());
        await dbContext.SaveChangesAsync();
    }
}

