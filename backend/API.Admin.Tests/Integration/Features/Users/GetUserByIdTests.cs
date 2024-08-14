using API.Admin.Feature.Users.GetUser;
using API.Admin.Tests.Integration.Features.Fakes;
using API.Admin.Tests.Integration.Utilities;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users;

public class GetUserByIdTests : IClassFixture<DatabaseFixture>
{
    private readonly HttpClient _client;
    private readonly DatabaseFixture _fixture;

    public GetUserByIdTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Auth
        var token = await _fixture.GetAuthAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        // Arrange
        var command = UserFake.CreateUserCommand();

        var result = await _client.GetAsync("/api/v1/user/C523CF8F-9230-4FA1-9B2A-378D16FD0822");
        var json = await _client.GetFromJsonAsync<ApiResult<UserQueryModel>>("/api/v1/user/C523CF8F-9230-4FA1-9B2A-378D16FD0822");
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuário recuperado com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);
    }
}
