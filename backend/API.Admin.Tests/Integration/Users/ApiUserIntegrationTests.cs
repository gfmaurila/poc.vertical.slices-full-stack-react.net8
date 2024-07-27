using API.Admin.Feature.Users.CreateUser;
using API.Admin.Feature.Users.DeleteUser;
using API.Admin.Feature.Users.GetUser;
using API.Admin.Feature.Users.UpdateEmail;
using API.Admin.Feature.Users.UpdatePassword;
using API.Admin.Feature.Users.UpdateUser;
using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Users.Data;
using API.Admin.Tests.Integration.Users.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using poc.core.api.net8.Response;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Users;

public class ApiUserIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ApiUserIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _auth = new AuthToken();
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region GET ALL
    [Fact(DisplayName = "01 - API Get User null")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GetUserNull()
    {
        // Limpa a base
        await UserMockData.DeleteUser(_factory, true);
        var url = "/api/v1/user";

        var token = await _auth.GetAuthAsync(_factory, _client);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserRepo.ClearDatabaseAsync(_factory);

        var result = await _client.GetAsync(url);
        var json = await _client.GetFromJsonAsync<ApiResult<List<UserQueryModel>>>(url);

        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Nenhum usuário encontrado.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);
    }

    [Fact(DisplayName = "02 - API Get User")]
    [Trait("Integração", "GetUserEndpoint")]
    public async Task GetUser()
    {
        await UserMockData.DeleteUser(_factory, true);
        await UserMockData.CreateUser(_factory, true);

        var token = await _auth.GetAuthAsync(_factory, _client);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        var url = "/api/v1/user";

        var result = await _client.GetAsync(url);
        var json = await _client.GetFromJsonAsync<ApiResult<List<UserQueryModel>>>(url);

        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuários recuperados com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region GET By ID
    [Fact(DisplayName = "03 - API Get User By Id")]
    [Trait("Integração", "GetUserByIdEndpoint")]
    public async Task GetUserById()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var id = await UserMockData.CreateUser(_factory);

        var url = $"/api/v1/user/{id}";

        var result = await _client.GetAsync(url);
        var json = await _client.GetFromJsonAsync<ApiResult<UserQueryModel>>(url);
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(json);
        Assert.Equal("Usuário recuperado com sucesso.", json.SuccessMessage);
        Assert.True(json.Success);
        Assert.Empty(json.Errors);

        // Limpa a base
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region POST
    [Fact(DisplayName = "04 – API Create User")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task CreateUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var command = UserFake.CreateUserCommand();
        var url = "/api/v1/user";

        // Envia o comando para criar um usuário
        var response = await _client.PostAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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

    [Fact(DisplayName = "05 – API Create User ERROR")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task CreateUserError1()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var command = UserFake.CreateUserInvalidDataCommand();
        var url = "/api/v1/user";

        var response = await _client.PostAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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

    [Fact(DisplayName = "06 – API Create User ERROR")]
    [Trait("Integracao", "CreateUserEndpoint")]
    public async Task CreateUserError2()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);


        await UserMockData.DeleteUser(_factory, true);
        await UserMockData.CreateUserExistingData(_factory);

        var command = UserFake.CreateUserExistingDataCommand();
        var url = "/api/v1/user";

        var response = await _client.PostAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
    #endregion

    #region PUT - Update
    [Fact(DisplayName = "07 - API Update User")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task UpdateUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateUserCommand(idUser);
        var url = "/api/v1/user";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "08 - API Update User ERROR")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task UpdateUserError1()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateUserInvalidDataCommand(idUser);
        var url = "/api/v1/user";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "09 - API Update User ERROR")]
    [Trait("Integração", "UpdateUserEndpoint")]
    public async Task UpdateUserError2()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateUserCommand(id);
        var url = "/api/v1/user";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region PUT - Password
    [Fact(DisplayName = "10 - API Update Password User")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task UpdatePasswordUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdatePasswordUserCommand(idUser);
        var url = "/api/v1/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "11 - API Update Password User ERROR")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task UpdatePasswordUserError1()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdatePasswordUserInvalidCommand(idUser);
        var url = "/api/v1/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "12 - API Update Password User ERROR")]
    [Trait("Integração", "UpdatePasswordUserEndpoint")]
    public async Task UpdatePasswordUserError2()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdatePasswordUserCommand(id);
        var url = "/api/v1/user/updatepassword";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region PUT - Email
    [Fact(DisplayName = "13 - API Update Email")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task UpdateEmail()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateEmailUserCommand(idUser);
        var url = "/api/v1/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "14 - API Update Email ERROR")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task UpdateEmailError1()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateEmailUserInvalidCommand(idUser);
        var url = "/api/v1/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "15 - API Update Email ERROR")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task UpdateEmailError2()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateEmailUserCommand(id);
        var url = "/api/v1/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);

        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "16 - API Update Email ERROR")]
    [Trait("Integração", "UpdateEmailUserEndpoint")]
    public async Task UpdateEmailError3()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var faker = new Faker("pt_BR");

        string email = faker.Person.Email;

        // Email ja existente
        await UserMockData.CreateUser(_factory, email);

        var id = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateEmailUserCommand(id, email);
        var url = "/api/v1/user/updateemail";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region PUT - ROLE
    [Fact(DisplayName = "17 - API Update Role User")]
    [Trait("Integração", "UpdateRoleUserEndpoint")]
    public async Task UpdateRoleUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = UserFake.UpdateEmailUserCommand(idUser);
        var url = "/api/v1/user/updaterole";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "18 - API Update Role User ERROR")]
    [Trait("Integração", "UpdateRoleUserEndpoint")]
    public async Task UpdateRoleUserError1()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        Guid id = Guid.NewGuid();

        var command = UserFake.UpdateEmailUserCommand(id);
        var url = "/api/v1/user/updaterole";

        // Envia o comando para criar um usuário
        var response = await _client.PutAsJsonAsync(url, command);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }

    [Fact(DisplayName = "19 - API Update Role User ERROR")]
    [Trait("Integração", "DeleteUserEndpoint")]
    public async Task UpdateRoleUserError2()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var idUser = await UserMockData.CreateUser(_factory);

        var command = new DeleteUserCommand(idUser);
        var url = $"/api/v1/user/{idUser}";

        // Envia o comando para criar um usuário
        var response = await _client.DeleteAsync(url);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

    #region DELETE
    [Fact(DisplayName = "20 - API Delete User")]
    [Trait("Integração", "DeleteUserEndpoint")]
    public async Task DeleteUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        await UserMockData.DeleteUser(_factory, true);

        var id = Guid.NewGuid();

        var command = new DeleteUserCommand(id);
        var url = $"/api/v1/user/{id}";

        // Envia o comando para criar um usuário
        var response = await _client.DeleteAsync(url);
        _client.DefaultRequestHeaders.Clear();

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
        await UserMockData.DeleteUser(_factory, true);
    }
    #endregion

}
