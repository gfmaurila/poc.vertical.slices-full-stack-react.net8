using API.Admin.Tests.Integration.Utilities.Auth;

namespace API.Admin.Tests.Integration.Utilities;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public HttpClient Client { get; }
    private readonly AuthToken1 _auth;

    private static Random random = new Random();

    public DatabaseFixture()
    {
        _auth = new AuthToken1();
        _factory = new TestWebApplicationFactory<Program>();
        Client = _factory.CreateClient();
    }

    public async Task InitializeAsync() { }

    public TestWebApplicationFactory<Program> Factory()
    {
        return _factory;
    }

    public async Task DisposeAsync()
    {
        //await Dispose.DropTables(_factory);
    }

    public async Task<AuthResponse> GetAuthAsync()
    {
        return new AuthResponse()
        {
            AccessToken = GenerateJwtToken()
        };
    }

    public string GenerateJwtToken()
    {
        return _auth.GenerateJwtToken();
    }

}
