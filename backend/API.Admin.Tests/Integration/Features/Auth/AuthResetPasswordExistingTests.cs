using API.Admin.Feature.Auth.ResetPassword;
using API.Admin.Tests.Integration.Features.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Common.Net8.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth;

public class AuthResetPasswordExistingTests : IClassFixture<DatabaseSQLServerFixture>
{
    private readonly HttpClient _client;
    private readonly DatabaseSQLServerFixture _fixture;

    public AuthResetPasswordExistingTests(DatabaseSQLServerFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Arrange
        var command = AuthFake.AuthResetPasswordCommand();

        var httpResponse = await _client.PostAsJsonAsync("/api/v1/resetpassword", command);

        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<AuthResetPasswordResponse>>();

        //Assert
        Assert.NotNull(jsonResponse);
        Assert.False(jsonResponse.Success);
        Assert.NotEmpty(jsonResponse.Errors);
    }
}
