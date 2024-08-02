using API.Admin.Feature.Users.UpdateUser;
using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Features.Users.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users.UpdateUserEndpoint;

public class UpdateUserTests : IClassFixture<UseSqliteWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly UseSqliteWebApplicationFactory<Program> _factory;

    public UpdateUserTests(UseSqliteWebApplicationFactory<Program> factory)
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

        // Arrange

        //Arrange command
        var id = await UserRepo.GetUserById(_factory);

        var command = UserFake.UpdateUserCommand(id);

        var url = "/api/v1/user";

        // Envia o comando para criar um usuário
        var httpResponse = await _client.PutAsJsonAsync(url, command);
        httpResponse.EnsureSuccessStatusCode();
        _client.DefaultRequestHeaders.Clear();

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UpdateUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
