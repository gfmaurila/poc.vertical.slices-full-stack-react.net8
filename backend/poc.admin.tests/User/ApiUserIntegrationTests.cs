using poc.admin.Feature.Users.GetArticle;
using poc.admin.tests.User.Data;
using poc.admin.tests.User.Fakes;
using System.Net;
using System.Net.Http.Json;

namespace poc.admin.tests.User;

public class ApiCatalogoIntegrationTests
{
    [Fact(DisplayName = "01 - GET - Deve recuperar sem usuários a API")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GET_USER_NULL()
    {
        await using var application = new AdminApiApplication();

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
        var url = "/api/user";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var json = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(json.Count == 0);
    }

    [Fact(DisplayName = "02 - GET - Deve recuperar todos os usuários da API")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GET_ALL_USER()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);
        await UserMockData.CreateUser(application, true);
        var url = "/api/user";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var json = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.True(json.Count >= 1);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "03 - POST - Deve criar um usuário e verificar se é o único na lista")]
    [Trait("Integração", "CreateUserEndpoint")]
    public async Task POST_CREATE_NEW_USER()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var command = UserFake.CreateUserCommand();

        var client = application.CreateClient();
        var url = "/api/user";

        var response = await client.PostAsJsonAsync(url, command);

        var json = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(json.Count == 1);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

}
