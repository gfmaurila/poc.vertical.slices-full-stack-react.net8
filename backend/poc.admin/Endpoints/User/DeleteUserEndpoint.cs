using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.DeleteUser;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.User;

public class DeleteUserEndpoint : ICarterModule
{
    /// <summary>
    /// Deleta o registro pelo Id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.DeleteUser}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/user/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteUserCommand(id));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
        .WithName("DeleteUser")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Deletar usuário",
            Description = "deletar usuário",
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
