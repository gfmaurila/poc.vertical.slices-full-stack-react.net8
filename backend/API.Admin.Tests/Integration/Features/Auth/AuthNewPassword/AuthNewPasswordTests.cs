using API.Admin.Feature.Auth.AuthNewPassword;
using API.Admin.Tests.Integration.Features.Auth.Fakes;
using API.Admin.Tests.Integration.Features.Users.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth.ResetPasswordEndpoint;

public class AuthResetPasswordTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public AuthResetPasswordTests(UseSqliteWebApplicationFactory<Program> factory)
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
        var command = AuthFake.AuthNewPasswordCommand();

        var httpResponse = await _client.PostAsJsonAsync("/api/v1/newpassword", command);
        httpResponse.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResult<AuthNewPasswordResponse>>();

        //Assert
        Assert.NotNull(jsonResponse);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }


}