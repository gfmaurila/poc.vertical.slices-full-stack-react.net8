using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Features.Users.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users.UpdatePasswordUserEndpoint;

public class UpdatePasswordUserInvalidIdTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public UpdatePasswordUserInvalidIdTests(UseSqliteWebApplicationFactory<Program> factory)
    {
        _auth = new AuthToken();
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Arrange - Auth
        var token = await _auth.GetAuthAsync(_factory, _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Data.Token);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);

        //Arrange command
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

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
