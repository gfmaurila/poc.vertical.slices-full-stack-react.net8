using API.Admin.Feature.Users.DeleteUser;
using API.Admin.Tests.Integration.Features.Users.Data;
using API.Admin.Tests.Integration.Features.Users.Fakes;
using API.Admin.Tests.Integration.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace API.Admin.Tests.Integration.Features.Users.DeleteUserEndpoint;

public class DeleteUserTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public DeleteUserTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task ShouldUser()
    {
        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);

        var id = await UserRepo.PopulateTestDeleteData(_factory);

        // Arrange
        var command = CreateUserCommandFake.CreateUserCommand();

        var httpResponse = await _client.DeleteAsync($"/api/user/{id}");
        httpResponse.EnsureSuccessStatusCode();

        // Verifica se a resposta HTTP está correta
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        // Extrai o JSON da resposta
        var jsonResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<DeleteUserResponse>>();

        // Verifica se o JSON tem os resultados esperados
        Assert.NotNull(jsonResponse);
        Assert.Equal("Removido com sucesso!", jsonResponse.SuccessMessage);
        Assert.True(jsonResponse.Success);
        Assert.Empty(jsonResponse.Errors);

        // Limpa base
        await UserRepo.ClearDatabaseAsync(_factory);
        //_app.Dispose();
    }
}
