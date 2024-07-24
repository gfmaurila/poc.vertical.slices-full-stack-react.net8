using poc.admin.Feature.Users.CreateUser;
using poc.admin.Feature.Users.GetArticle;
using poc.admin.tests.User.Data;
using poc.admin.tests.User.Fakes;
using poc.core.api.net8.API.Models;
using poc.core.api.net8.Response;
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
        var json = await client.GetFromJsonAsync<ApiResult<List<UserQueryModel>>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Nenhum usuário encontrado.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);
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
        var json = await client.GetFromJsonAsync<ApiResult<List<UserQueryModel>>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuários recuperados com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "03 - GET - Deve recuperar usuários por id")]
    [Trait("Integração", "GetUserByIdEndpoint")]
    public async Task GET_USER_ID()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var id = await UserMockData.CreateUser(application);

        var url = $"/api/user/{id}";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var json = await client.GetFromJsonAsync<ApiResult<UserQueryModel>>(url);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuário recuperado com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "04 – POST – Deve criar um usuário")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task POST_CREATE_NEW_USER()
    {
        await using var application = new AdminApiApplication();
        await UserMockData.DeleteUser(application, true);

        var command = UserFake.CreateUserCommand();
        var client = application.CreateClient();
        var url = "/api/user";

        // Envia o comando para criar um usuário
        var response = await client.PostAsJsonAsync(url, command);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Cadastrado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);
    }

    [Fact(DisplayName = "05 – POST – Deve dar erro ao tentar criar um usuário")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task POST_CREATE_NEW_USER_INVALID_DATA()
    {
        await using var application = new AdminApiApplication();
        await UserMockData.DeleteUser(application, true);

        var command = UserFake.CreateUserInvalidDataCommand();
        var client = application.CreateClient();
        var url = "/api/user";

        var response = await client.PostAsJsonAsync(url, command);

        // Verifica se a resposta HTTP é 400 - Bad Request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            "'First Name' deve ser informado.",
            "'Last Name' deve ser informado.",
            "'Email' deve ser informado.",
            "'Email' é um endereço de email inválido.",
            "'Password' deve ser informado.",
            "'Password' deve ser maior ou igual a 8 caracteres. Você digitou 0 caracteres.",
            "'Password' não está no formato correto.",
            "'Password' não está no formato correto.",
            "'Password' não está no formato correto.",
            "'Password' não está no formato correto."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());

    }

    [Fact(DisplayName = "06 – POST – Deve dar erro ao tentar criar um usuário existente")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task POST_CREATE_NEW_USER_EXISTING_DATA()
    {
        await using var application = new AdminApiApplication();
        await UserMockData.DeleteUser(application, true);
        await UserMockData.CreateUserExistingData(application);

        var command = UserFake.CreateUserExistingDataCommand();
        var client = application.CreateClient();
        var url = "/api/user";

        var response = await client.PostAsJsonAsync(url, command);

        // Verifica se a resposta HTTP é 400 - Bad Request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            "O endereço de e-mail informado já está sendo utilizado."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());

    }

}
