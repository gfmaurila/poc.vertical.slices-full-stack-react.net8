using API.Admin.Feature.Auth.ResetPassword;
using API.Admin.Tests.Integration.Features.Auth.Fakes;
using API.Admin.Tests.Integration.Features.Users.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth.ResetPasswordEndpoint;

public class AuthResetPasswordExistingTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public AuthResetPasswordExistingTests(UseSqliteWebApplicationFactory<Program> factory)
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

        // Arrange
        var command = AuthFake.AuthResetPasswordCommand();

        var httpResponse = await _client.PostAsJsonAsync("/api/v1/resetpassword", command);

        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<AuthResetPasswordResponse>>();

        //Assert
        Assert.NotNull(jsonResponse);
        Assert.False(jsonResponse.Success);
        Assert.NotEmpty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
