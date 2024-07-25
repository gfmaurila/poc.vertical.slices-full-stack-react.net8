using API.Admin.Feature.Users.DeleteUser;
using API.Admin.Feature.Users.GetUser;
using API.Admin.Feature.Users.UpdateEmail;
using API.Admin.Feature.Users.UpdatePassword;
using API.Admin.Feature.Users.UpdateUser;
using API.Admin.Tests.User.Data;
using API.Admin.Tests.User.Fakes;
using Bogus;
using poc.core.api.net8.API.Models;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.User;

public class ApiCatalogoIntegrationTests
{
    // Não OK
    #region GET ALL
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
    #endregion

    #region GET By ID
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
    #endregion

    // OK
    #region POST
    //[Fact(DisplayName = "04 – POST – Deve criar um usuário")]
    //[Trait("Integracao", "CreateUserEndpoint")]
    //public async Task POST_CREATE_NEW_USER()
    //{
    //    await using var application = new AdminApiApplication();
    //    await UserMockData.DeleteUser(application, true);

    //    var command = UserFake.CreateUserCommand();
    //    var client = application.CreateClient();
    //    var url = "/api/user";

    //    // Envia o comando para criar um usuário
    //    var response = await client.PostAsJsonAsync(url, command);

    //    // Verifica se a resposta HTTP está correta
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    //    // Extrai o JSON da resposta
    //    var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateUserResponse>>();

    //    // Verifica se o JSON tem os resultados esperados
    //    Assert.NotNull(jsonResponse);
    //    Assert.Equal("Cadastrado com sucesso!", jsonResponse.SuccessMessage);
    //    Assert.True(jsonResponse.Success);
    //    Assert.Empty(jsonResponse.Errors);
    //}

    //[Fact(DisplayName = "05 – POST – Deve dar erro ao tentar criar um usuário")]
    //[Trait("Integracao", "CreateUserEndpoint")]
    //public async Task POST_CREATE_NEW_USER_INVALID()
    //{
    //    await using var application = new AdminApiApplication();
    //    await UserMockData.DeleteUser(application, true);

    //    var command = UserFake.CreateUserInvalidDataCommand();
    //    var client = application.CreateClient();
    //    var url = "/api/user";

    //    var response = await client.PostAsJsonAsync(url, command);

    //    // Verifica se a resposta HTTP é 400 - Bad Request
    //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    //    // Extrai o JSON da resposta
    //    var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

    //    // Verifica se o campo "success" é false
    //    Assert.False(jsonResponse.Success);

    //    // Verifica se a lista de erros contém as mensagens específicas
    //    var expectedErrors = new List<string>
    //    {
    //        "'First Name' deve ser informado.",
    //        "'Last Name' deve ser informado.",
    //        "'Email' deve ser informado.",
    //        "'Email' é um endereço de email inválido.",
    //        "'Password' deve ser informado.",
    //        "'Password' deve ser maior ou igual a 8 caracteres. Você digitou 0 caracteres.",
    //        "'Password' não está no formato correto.",
    //        "'Password' não está no formato correto.",
    //        "'Password' não está no formato correto.",
    //        "'Password' não está no formato correto."
    //    };

    //    Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

    //    // Verifica se a quantidade de erros é a esperada
    //    Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());

    //}

    //[Fact(DisplayName = "06 – POST – Deve dar erro ao tentar criar um usuário existente")]
    //[Trait("Integracao", "CreateUserEndpoint")]
    //public async Task POST_CREATE_NEW_USER_EXISTING()
    //{
    //    await using var application = new AdminApiApplication();
    //    await UserMockData.DeleteUser(application, true);
    //    await UserMockData.CreateUserExistingData(application);

    //    var command = UserFake.CreateUserExistingDataCommand();
    //    var client = application.CreateClient();
    //    var url = "/api/user";

    //    var response = await client.PostAsJsonAsync(url, command);

    //    // Verifica se a resposta HTTP é 400 - Bad Request
    //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    //    // Extrai o JSON da resposta
    //    var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

    //    // Verifica se o campo "success" é false
    //    Assert.False(jsonResponse.Success);

    //    // Verifica se a lista de erros contém as mensagens específicas
    //    var expectedErrors = new List<string>
    //    {
    //        "O endereço de e-mail informado já está sendo utilizado."
    //    };

    //    Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

    //    // Verifica se a quantidade de erros é a esperada
    //    Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());

    //}
    #endregion

    #region PUT - Update
    [Fact(DisplayName = "07 - PUT - Deve alterar usuários por id")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task PUT_USER_ID()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateUserCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "08 - PUT - Deve dar erro ao tentar alterar um usuário")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task PUT_USER_ID_INVALID()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateUserInvalidDataCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            "'First Name' deve ser informado.",
            "'Last Name' deve ser informado."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "09 - PUT - Deve dar erro ao tentar alterar um usuário")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task PUT_USER_ID_NOT_FOUND()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateUserCommand(id);
        var client = application.CreateClient();
        var url = "/api/user";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            $"Nenhum registro encontrado pelo Id: {id}"
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }
    #endregion

    #region PUT - Password
    [Fact(DisplayName = "10 - PUT - Deve alterar senha de usuário")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task PUT_USER_PASSWORD()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdatePasswordUserCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdatePasswordUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "11 - PUT - Deve dar erro ao tentar alterar senha de usuário")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task PUT_USER_PASSWORD_INVALID()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdatePasswordUserInvalidCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            "A confirmação da senha deve ser igual à senha."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "12 - PUT - Deve dar erro ao tentar alterar um usuário")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task PUT_USER_PASSWORD_ID_NOT_FOUND()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdatePasswordUserCommand(id);
        var client = application.CreateClient();
        var url = "/api/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            $"Nenhum registro encontrado pelo Id: {id}"
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }
    #endregion

    #region PUT - Email
    [Fact(DisplayName = "13 - PUT - Deve alterar EMAIL de usuário")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task PUT_USER_EMAIL()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateEmailUserCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateEmailUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "14 - PUT - Deve dar erro ao tentar alterar email de usuário")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task PUT_USER_EMAIL_INVALID()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateEmailUserInvalidCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            "'Email' é um endereço de email inválido."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "15 - PUT - Deve dar erro ao tentar alterar email de usuário")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task PUT_USER_EMAIL_ID_NOT_FOUND()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateEmailUserCommand(id);
        var client = application.CreateClient();
        var url = "/api/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            $"Nenhum registro encontrado pelo Id: {id}"
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "16 - PUT - Deve dar erro ao tentar alterar email de usuário")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task PUT_USER_EMAIL_EXISTING_FOUND()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var faker = new Faker("pt_BR");

        string email = faker.Person.Email;

        // Email ja existente
        await UserMockData.CreateUser(application, email);

        var id = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateEmailUserCommand(id, email);
        var client = application.CreateClient();
        var url = "/api/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            $"O endereço de e-mail informado já está sendo utilizado."
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }
    #endregion

    #region PUT - ROLE
    [Fact(DisplayName = "17 - PUT - Deve alterar Role de usuário")]
    [Trait("Integração", "UpdateRoleUserEndpoint")]
    public async Task PUT_USER_ROLE()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = UserFake.UpdateEmailUserCommand(idUser);
        var client = application.CreateClient();
        var url = "/api/user/updaterole";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateEmailUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "18 - PUT - Deve dar erro ao tentar alterar role de usuário")]
    [Trait("Integração", "UpdateRoleUserEndpoint")]
    public async Task PUT_USER_ROLE_ID_NOT_FOUND()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateEmailUserCommand(id);
        var client = application.CreateClient();
        var url = "/api/user/updaterole";

        // Envia o comando para criar um usuário
        var response = await client.PutAsJsonAsync(url, command);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Verifica se o campo "success" é false
        Assert.False(jsonResponse.Success);

        // Verifica se a lista de erros contém as mensagens específicas
        var expectedErrors = new List<string>
        {
            $"Nenhum registro encontrado pelo Id: {id}"
        };

        Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

        // Verifica se a quantidade de erros é a esperada
        Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }

    [Fact(DisplayName = "19 - DELETE - Deve deletar usuário")]
    [Trait("Integração", "DeleteUserEndpoint")]
    public async Task DELETE_USER_ROLE()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.DeleteUser(application, true);

        var idUser = await UserMockData.CreateUser(application);

        var command = new DeleteUserCommand(idUser);
        var client = application.CreateClient();
        var url = $"/api/user/{idUser}";

        // Envia o comando para criar um usuário
        var response = await client.DeleteAsync(url);

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeleteUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Removido com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(application, true);
    }
    #endregion

    // OK
    #region DELETE
    //[Fact(DisplayName = "20 - DELETE - Deve retornar erro ao tentar remover usuário")]
    //[Trait("Integração", "DeleteUserEndpoint")]
    //public async Task DELETE_USER_ID_ROLE()
    //{
    //    await using var application = new AdminApiApplication();

    //    await UserMockData.DeleteUser(application, true);

    //    var id = Guid.NewGuid();

    //    var command = new DeleteUserCommand(id);
    //    var client = application.CreateClient();
    //    var url = $"/api/user/{id}";

    //    // Envia o comando para criar um usuário
    //    var response = await client.DeleteAsync(url);

    //    // Extrai o JSON da resposta
    //    var jsonResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

    //    // Verifica se o campo "success" é false
    //    Assert.False(jsonResponse.Success);

    //    // Verifica se a lista de erros contém as mensagens específicas
    //    var expectedErrors = new List<string>
    //    {
    //        $"Nenhum registro encontrado pelo Id: {id}"
    //    };

    //    Assert.All(expectedErrors, error => Assert.Contains(jsonResponse.Errors.Select(e => e.Message), e => e == error));

    //    // Verifica se a quantidade de erros é a esperada
    //    Assert.Equal(expectedErrors.Count, jsonResponse.Errors.Count());


    //    // Limpa a base
    //    await UserMockData.DeleteUser(application, true);
    //}
    #endregion

}
