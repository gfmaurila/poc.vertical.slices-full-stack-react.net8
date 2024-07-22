using poc.admin.Feature.Users.GetArticle;
using poc.admin.tests.API.Database;
using System.Net;
using System.Net.Http.Json;

namespace poc.admin.tests.API.User;

public class ApiCatalogoIntegrationTests
{
    [Fact(DisplayName = "Deve recuperar todos os usuários da API")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GET_ALL_USER()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.CreateUser(application, true);
        var url = "/api/user";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var json = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.True(json.Count == 1);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "Deve recuperar sem usuários a API")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GET_USER_NULL()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.CreateUser(application, false);
        var url = "/api/user";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var json = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(json.Count == 1);
    }


}
