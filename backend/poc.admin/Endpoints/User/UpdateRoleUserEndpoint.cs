using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.UpdateRole;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class UpdateRoleUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/user/updaterole", HandleUpdateUserRole)
            .WithName("UpdateUserRole")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Atualizar permissões de usuário",
                Description = "Atualiza as permissões de um usuário existente, identificado pelo ID no corpo da requisição. Permite modificar o nível de acesso ou as roles atribuídas ao usuário.",
                Tags = new List<OpenApiTag>
                {
                    new OpenApiTag
                    {
                        Name = "Usuários"
                    }
                }
            })
            //.RequireAuthorization(new AuthorizeAttribute { Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}" })
            ;
    }

    private async Task<IResult> HandleUpdateUserRole(UpdateRoleUserCommand command, ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.Success)
            return Results.BadRequest(result);

        return Results.Ok(result);
    }
}


//public class UpdateRoleUserEndpoint : ICarterModule
//{

//    /// <summary>
//    /// Atualiza um registro existente.
//    /// </summary>
//    /// <param name="command"></param>
//    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
//    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
//    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
//    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
//    [HttpPut("updaterole")]
//    [Consumes(MediaTypeNames.Application.Json)]
//    [Produces(MediaTypeNames.Application.Json)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
//    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapPut("api/user/updaterole", async (UpdateRoleUserCommand request, ISender sender) =>
//        {
//            var result = await sender.Send(request.Adapt<UpdateRoleUserCommand>());
//            if (!result.IsSuccess)
//                return Results.BadRequest(result.Errors);

//            return Results.Ok(result.Value);
//        })
//        .WithName("PutUserRole")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Atualizar permissões de usuário",
//            Description = "Atualizar permissões de usuário",
//            Tags = new List<OpenApiTag>
//            {
//                new OpenApiTag
//                {
//                    Name = "Usuários"
//                }
//            }
//        });
//    }
//}

