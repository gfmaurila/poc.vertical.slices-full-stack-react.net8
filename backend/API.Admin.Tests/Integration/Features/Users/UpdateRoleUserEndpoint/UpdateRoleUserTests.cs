using API.Admin.Feature.Users.UpdateEmail;
using API.Admin.Tests.Integration.Features.Auth.AuthEndpoint;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Features.Users.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users.UpdateRoleUserEndpoint;

public class UpdateRoleUserTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly AuthToken _auth;
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UpdateRoleUserTests(CustomWebApplicationFactory<Program> factory)
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
        var id = await UserRepo.GetUserById(_factory);

        var command = UserFake.UpdateEmailUserCommand(id);

        var url = "/api/v1/user/updaterole";

        // Envia o comando para criar um usuário
        var httpResponse = await _client.PutAsJsonAsync(url, command);
        httpResponse.EnsureSuccessStatusCode();
        _client.DefaultRequestHeaders.Clear();

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UpdateEmailUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Atualizado com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
    }
}
