using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.UpdateEmail;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.User;
public class UpdateEmailUserEndpoint : ICarterModule
{

    /// <summary>
    /// Atualiza um registro existente.
    /// </summary>
    /// <param name="command"></param>
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
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/user/updateemail", async (UpdateEmailUserCommand request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<UpdateEmailUserCommand>());
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
        .WithName("PutUserEmail")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Atualizar email de usuário",
            Description = "Atualizar email de usuário",
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

