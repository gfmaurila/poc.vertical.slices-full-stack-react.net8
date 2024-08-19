using API.Admin.Infrastructure.Database;
using API.Admin.Tests.Integration.Utilities.Auth;

namespace API.Admin.Tests.Integration.Utilities;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public HttpClient Client { get; }
    private readonly AuthToken1 _auth;
    private EFSqlServerContext _context;

    private static Random random = new Random();

    public DatabaseFixture()
    {
        _auth = new AuthToken1();
        _factory = new TestWebApplicationFactory<Program>();
        Client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        //await _context.Database.EnsureDeletedAsync();
        //await _context.Database.MigrateAsync();
    }

    public TestWebApplicationFactory<Program> Factory()
    {
        return _factory;
    }

    public async Task DisposeAsync()
    {
        //await _context.Database.EnsureDeletedAsync();
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
