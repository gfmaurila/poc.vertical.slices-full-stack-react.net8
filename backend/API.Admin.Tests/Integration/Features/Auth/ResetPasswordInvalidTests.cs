using API.Admin.Feature.Auth.ResetPassword;
using API.Admin.Tests.Integration.Features.Fakes;
using API.Admin.Tests.Integration.Utilities;
using poc.core.api.net8.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Auth;

public class ResetPasswordInvalidTests : IClassFixture<DatabaseSQLServerFixture>
{
    private readonly HttpClient _client;
    private readonly DatabaseSQLServerFixture _fixture;

    public ResetPasswordInvalidTests(DatabaseSQLServerFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Arrange
        var command = AuthFake.AuthResetPasswordInvalidCommand();

        var httpResponse = await _client.PostAsJsonAsync("/api/v1/resetpassword", command);

        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<AuthResetPasswordResponse>>();

        //Assert
        Assert.NotNull(jsonResponse);
        Assert.False(jsonResponse.Success);
        Assert.NotEmpty(jsonResponse.Errors);
    }
}
