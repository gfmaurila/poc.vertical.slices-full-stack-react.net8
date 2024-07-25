using API.Admin.Feature.Users.GetUser;
using API.Admin.Infrastructure.Database;
using API.Admin.Tests.Redis;
using API.Admin.Tests.User.Fakes;
using Microsoft.Extensions.DependencyInjection;
using poc.core.api.net8.Interface;
using poc.core.api.net8.ValueObjects;

namespace API.Admin.Tests.User.Data;

public class UserMockData
{
    public static async Task CreateUser(AdminApiApplication application, bool create)
    {
        using var scope = application.Services.CreateScope();
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

    public static async Task<Guid> CreateUser(AdminApiApplication application)
    {
        using var scope = application.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var user = await dbContext.User.AddAsync(UserFake.Insert());
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static async Task<Guid> CreateUser(AdminApiApplication application, string email)
    {
        using var scope = application.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var emailL = new Email(email);

        var user = await dbContext.User.AddAsync(UserFake.Insert(emailL));
        await dbContext.SaveChangesAsync();

        return user.Entity.Id;
    }

    public static async Task CreateUserExistingData(AdminApiApplication application)
    {
        using var scope = application.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.User.AddAsync(UserFake.InsertExistingData());
        await dbContext.SaveChangesAsync();
    }

    public static async Task DeleteUser(AdminApiApplication application, bool delete)
    {
        using var scope = application.Services.CreateScope();
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
}
