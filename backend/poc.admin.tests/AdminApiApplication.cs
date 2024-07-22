using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using poc.admin.Infrastructure.Database;

namespace poc.admin.tests;


public class AdminApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Carregar as configurações do arquivo appsettings.Test.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        // Obter a string de conexão do arquivo de configuração
        var connectionString = configuration.GetConnectionString("SqlConnection");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EFSqlServerContext>));
            services.AddDbContext<EFSqlServerContext>(options => options.UseSqlServer(connectionString));
        });

        return base.CreateHost(builder);
    }
}