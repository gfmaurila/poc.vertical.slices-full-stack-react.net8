using Microsoft.Extensions.Configuration;

namespace API.Admin.Tests;
public static class AdminDb
{
    public static string ConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

        return configuration.GetConnectionString("SqlConnection");
    }
}
