using Microsoft.Extensions.DependencyInjection;
using poc.admin.Feature.Users.GetArticle;
using poc.admin.Infrastructure.Database;
using poc.admin.tests.Redis;
using poc.admin.tests.User.Fakes;
using poc.core.api.net8.Interface;

namespace poc.admin.tests.User.Data;

public class UserMockData
{
    public static async Task CreateUser(AdminApiApplication application, bool create)
    {
        // https://github.com/macoratti/CatalogoApi_TestesIntegracao

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
