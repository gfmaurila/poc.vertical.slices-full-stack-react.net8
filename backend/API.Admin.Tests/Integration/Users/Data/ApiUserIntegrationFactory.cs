using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Admin.Tests.Integration.Users.Data;

public class ApiUserIntegrationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        builder.UseEnvironment("Test");

        //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Docker");
        //builder.UseEnvironment("Docker");

        //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        //builder.UseEnvironment("Development");

        //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
        //builder.UseEnvironment("Production");

        //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");
        //builder.UseEnvironment("Staging");
    }
}
