using Carter;
using Mapster;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Articles;
using poc.admin.Feature.Users.CreateUser;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;



public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/User", HandleCreateUser)
            .WithName("CreateUser")
            .Produces<ArticleResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.OperationId = "CreateUser";
                x.Summary = "Inserir usuários";
                x.Description = "Cadastra um novo usuário no sistema.";
                x.Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Usuários" } };
                return x;
            })
            //.RequireAuthorization(new AuthorizeAttribute {Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PostUser}" })
            ;
    }

    private async Task<IResult> HandleCreateUser(CreateUserCommand command, ISender sender)
    {
        var result = await sender.Send(command.Adapt<CreateUserCommand>());
        if (!result.IsSuccess)
            return Results.BadRequest(result.Errors);
        return Results.Ok(result.Value);
    }
}


//public class CreateUserEndpoint : ICarterModule
//{
//    /// <summary>
//    /// Cadastra um novo registro.
//    /// </summary>
//    /// <param name="command"></param>
//    /// <response code="200">Retorna o Id do novo registro.</response>
//    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
//    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
//    [HttpPost("api/user")]
//    [Consumes(MediaTypeNames.Application.Json)]
//    [Produces(MediaTypeNames.Application.Json)]
//    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)] // Adapte o tipo conforme necessário
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
//    [Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PostUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapPost("api/User", async (CreateUserCommand command, ISender sender) =>
//        {
//            var result = await sender.Send(command.Adapt<CreateUserCommand>());
//            if (!result.IsSuccess)
//                return Results.BadRequest(result.Errors);
//            return Results.Ok(result.Value);
//        })
//        .WithName("CreateUser")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Inserir usuários",
//            Description = "Inserir usuários",
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