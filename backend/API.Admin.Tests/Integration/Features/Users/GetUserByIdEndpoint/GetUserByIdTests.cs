using API.Admin.Feature.Users.GetUser;
using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Features.Users.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users.GetUserByIdEndpoint;

public class GetUserByIdTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public GetUserByIdTests(CustomWebApplicationFactory<Program> factory)
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
        await UserRepo.ClearDatabaseAsync(_factory);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);

        var id = await UserRepo.GetUserById(_factory);

        // Arrange
        var command = UserFake.CreateUserCommand();

        var result = await _client.GetAsync($"/api/v1/user/{id}");
        var json = await _client.GetFromJsonAsync<ApiResult<UserQueryModel>>($"/api/v1/user/{id}");
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuário recuperado com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
