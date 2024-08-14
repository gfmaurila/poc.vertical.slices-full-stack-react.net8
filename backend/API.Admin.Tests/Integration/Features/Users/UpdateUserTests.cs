using API.Admin.Feature.Users.UpdateUser;
using API.Admin.Tests.Integration.Features.Fakes;
using API.Admin.Tests.Integration.Utilities.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using poc.core.api.net8.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using API.Admin.Tests.Integration.Utilities;

namespace API.Admin.Tests.Integration.Features.Users;

public class UpdateUserTests : IClassFixture<DatabaseFixture>
{
    private readonly HttpClient _client;
    private readonly DatabaseFixture _fixture;

    public UpdateUserTests(DatabaseFixture fixture)
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
        var id = await UserFake.GetUserById(_fixture);

        var command = UserFake.UpdateUserCommand(id);

        // Envia o comando para criar um usuário
        var httpResponse = await _client.PutAsJsonAsync("/api/v1/user", command);
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

        await UserFake.Delete(_fixture, _client, id);
    }
}
