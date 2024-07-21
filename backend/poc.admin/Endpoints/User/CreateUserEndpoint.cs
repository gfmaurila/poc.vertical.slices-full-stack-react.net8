using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using poc.admin.Feature.Articles;
using poc.admin.Feature.Users.CreateUser;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.User;
public class CreateUserEndpoint : ICarterModule
{
    /// <summary>
    /// Cadastra um novo registro.
    /// </summary>
    /// <param name="command"></param>
    /// <response code="200">Retorna o Id do novo registro.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpPost("api/user")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)] // Adapte o tipo conforme necessário
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/User", async (CreateUserCommand command, ISender sender) =>
        {
            var result = await sender.Send(command.Adapt<CreateUserCommand>());
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);
            return Results.Ok(result.Value);
        });
    }
}

