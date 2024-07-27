using API.Admin.Feature.Users.UpdateUser;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/user", HandleUpdateUser)
            .WithName("UpdateUser")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Atualizar dados do usuário",
                Description = "Atualiza os dados de um usuário existente, utilizando o ID fornecido no corpo da requisição para identificação. Os dados atualizáveis incluem nome, email, senha, entre outros campos pertinentes.",
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

    private async Task<IResult> HandleUpdateUser(UpdateUserCommand command, ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.Success)
            return Results.BadRequest(result);

        return Results.Ok(result);
    }
}



//public class UpdateUserEndpoint : ICarterModule
//{

//    /// <summary>
//    /// Atualiza um registro existente.
//    /// </summary>
//    /// <param name="request">Dados do artigo para atualização.</param>
//    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
//    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
//    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
//    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
//    [HttpPut("api/v1/user")]
//    [Consumes(MediaTypeNames.Application.Json)]
//    [Produces(MediaTypeNames.Application.Json)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
//    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapPut("api/v1/user", async (UpdateUserCommand request, ISender sender) =>
//        {
//            var result = await sender.Send(request.Adapt<UpdateUserCommand>());
//            if (!result.IsSuccess)
//                return Results.BadRequest(result.Errors);

//            return Results.Ok(result.Value);
//        })
//        .WithName("PutUser")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Atualizar usuário",
//            Description = "Atualizar usuário",
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

