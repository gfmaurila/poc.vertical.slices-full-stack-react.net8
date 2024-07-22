using Microsoft.Extensions.DependencyInjection;
using poc.admin.Infrastructure.Database;
using poc.admin.tests.Fakes;

namespace poc.admin.tests.API.Database;

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
    }
}
