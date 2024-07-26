using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Admin.Tests.Integration.Utilities;

public class TestServerFixture : WebApplicationFactory<ApiApplication>
{
    public HttpClient Client { get; private set; }
    public ApiApplication _app { get; private set; }

    public TestServerFixture()
    {
        _app = new ApiApplication();
        Client = CreateClient();
    }

    public HttpClient CreateClient()
    {
        return base.CreateClient();
    }
}
