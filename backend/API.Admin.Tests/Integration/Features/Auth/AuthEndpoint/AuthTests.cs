using API.Admin.Feature.Auth.Login;
using API.Admin.Tests.Integration.Features.Auth.Fakes;
using API.Admin.Tests.Integration.Features.Users.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;

public class AuthTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public AuthTests(UseSqliteWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
        await UserRepo.GetUserById(_factory);

        // Arrange
        var command = AuthFake.AuthCommand();

        var httpResponse = await _client.PostAsJsonAsync("/api/v1/login", command);
        httpResponse.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResult<AuthTokenResponse>>();

        //Assert
        Assert.NotNull(jsonResponse);
        Assert.Equal("Sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }


}