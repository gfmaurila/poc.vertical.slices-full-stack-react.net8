using API.Admin.Feature.Users.GetUser;
using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Headers;

namespace API.Admin.Tests.Integration.Features.Users.GetUserEndpoint;

public class GetUserTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public GetUserTests(UseSqliteWebApplicationFactory<Program> factory)
    {
        _auth = new AuthToken();
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);

        // Arrange
        await UserRepo.GetUserById(_factory);

        var httpResponse = await _client.GetAsync("/api/v1/user");
        httpResponse.EnsureSuccessStatusCode();
        var stringResponse = await httpResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResult<List<UserQueryModel>>>(stringResponse);
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.NotNull(result);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
