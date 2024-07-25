using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.DeleteUser;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/user/{id}", HandleDeleteUser)
            .WithName("DeleteUser")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
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
             })
            //.RequireAuthorization(new AuthorizeAttribute { Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.DeleteUser}" })
            ;
    }

    private async Task<IResult> HandleDeleteUser(Guid id, ISender sender)
    {
        var result = await sender.Send(new DeleteUserCommand(id));

        if (!result.Success)
            return Results.BadRequest(result);

        return Results.Ok(result);
    }
}


//public class DeleteUserEndpoint : ICarterModule
//{
//    /// <summary>
//    /// Deleta o registro pelo Id.
//    /// </summary>
//    /// <param name="id"></param>
//    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
//    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
//    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
//    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
//    [Consumes(MediaTypeNames.Application.Json)]
//    [Produces(MediaTypeNames.Application.Json)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
//    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.DeleteUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapDelete("api/user/{id}", async (Guid id, ISender sender) =>
//        {
//            var result = await sender.Send(new DeleteUserCommand(id));

//            if (!result.IsSuccess)
//                return Results.BadRequest(result.Errors);

//            return Results.Ok(result.Value);
//        })
//        .WithName("DeleteUser")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Deletar usuário",
//            Description = "deletar usuário",
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
