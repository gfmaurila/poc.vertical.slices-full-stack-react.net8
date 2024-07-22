using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.UpdatePassword;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class UpdatePasswordUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/user/updatepassword", HandleUpdatePassword)
            .WithName("UpdateUserPassword")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Atualizar senha de usuário",
                Description = "Atualiza a senha de um usuário existente. A requisição deve conter o ID do usuário e a nova senha.",
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

    private async Task<IResult> HandleUpdatePassword(UpdatePasswordUserCommand command, ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
            return Results.BadRequest(result.Errors);

        return Results.Ok(result.Value);
    }
}



//public class UpdatePasswordUserEndpoint : ICarterModule
//{

//    /// <summary>
//    /// Atualiza um registro existente.
//    /// </summary>
//    /// <param name="command"></param>
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
//    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapPut("api/user/updatepassword", async (UpdatePasswordUserCommand request, ISender sender) =>
//        {
//            var result = await sender.Send(request.Adapt<UpdatePasswordUserCommand>());
//            if (!result.IsSuccess)
//                return Results.BadRequest(result.Errors);

//            return Results.Ok(result.Value);
//        })
//        .WithName("PutUpdatePasswordUser")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Atualizar senha de usuário",
//            Description = "Atualizar senha de usuário",
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

