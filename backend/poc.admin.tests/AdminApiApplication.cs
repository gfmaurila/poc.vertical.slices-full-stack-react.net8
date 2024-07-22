using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using poc.admin.Infrastructure.Database;

namespace poc.admin.tests;


public class AdminApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EFSqlServerContext>));

            services.AddDbContext<EFSqlServerContext>(options =>
                options.UseInMemoryDatabase("core_test", root));
        });

        return base.CreateHost(builder);
    }
}