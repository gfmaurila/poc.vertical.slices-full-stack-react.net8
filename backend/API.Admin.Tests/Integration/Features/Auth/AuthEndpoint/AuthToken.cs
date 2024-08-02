using API.Admin.Feature.Auth.Login;
using API.Admin.Tests.Integration.Features.Auth.Fakes;
using API.Admin.Tests.Integration.Features.Users.Data;
using poc.core.api.net8.Response;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;

public class AuthToken
{
    public async Task<ApiResult<AuthTokenResponse>> GetAuthAsync(UseSqliteWebApplicationFactory<Program> factory, HttpClient client)
    {
        var command = AuthFake.GetAuthAsync();
        await UserRepo.GetAuth(factory);
        var httpResponse = await client.PostAsJsonAsync("/api/v1/login", command);
        return await httpResponse.Content.ReadFromJsonAsync<ApiResult<AuthTokenResponse>>();
    }
}
