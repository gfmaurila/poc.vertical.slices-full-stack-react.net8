using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.GetArticle;
using poc.admin.Feature.Users.GetUserById;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.User;

public class GetUserByIdEndpoint : ICarterModule
{

    /// <summary>
    /// Obtém o registro pelo Id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Retorna o registro.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserQueryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.GetUserById}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/user/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetUserByIdQuery(id);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
        .WithName("GetByIdUser")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Buscar por id usuários",
            Description = "Buscar por id usuários",
            Tags = new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = "Usuários"
                }
            }
        });
    }
}
