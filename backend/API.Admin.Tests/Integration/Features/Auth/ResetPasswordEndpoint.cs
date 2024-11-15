using API.Admin.Feature.Auth.AuthNewPassword;
using API.Admin.Tests.Integration.Features.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Common.Net8.Response;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth;

public class AuthResetPasswordTests : IClassFixture<DatabaseSQLServerFixture>
{
    private readonly HttpClient _client;
    private readonly DatabaseSQLServerFixture _fixture;

    public AuthResetPasswordTests(DatabaseSQLServerFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task ShouldUser()
    {
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
    }
}
