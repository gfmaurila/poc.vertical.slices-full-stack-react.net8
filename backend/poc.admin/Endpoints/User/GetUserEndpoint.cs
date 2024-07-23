using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Users.GetArticle;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/user", HandleGetAllUsers)
            .WithName("GetAllUsers")
            .Produces<ApiResponse<List<UserQueryModel>>>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Buscar todos os usuários",
                Description = "Retorna uma lista com todos os usuários registrados no sistema.",
                Tags = new List<OpenApiTag>
                {
                    new OpenApiTag
                    {
                        Name = "Usuários"
                    }
                }
            })
            //.RequireAuthorization(new AuthorizeAttribute { Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.GetUser}" })
            ;
    }

    private async Task<IResult> HandleGetAllUsers(ISender sender)
    {
        var result = await sender.Send(new GetUserQuery());

        if (!result.Success)
            return Results.BadRequest(result.Errors);

        return Results.Ok(result.Data);
    }
}


//public class GetUserEndpoint : ICarterModule
//{
//    /// <summary>
//    /// Obtém uma lista com todos os registro.
//    /// </summary>
//    /// <response code="200">Retorna a lista de registro.</response>
//    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
//    [HttpGet]
//    [Consumes(MediaTypeNames.Application.Json)]
//    [Produces(MediaTypeNames.Application.Json)]
//    [ProducesResponseType(typeof(ApiResponse<List<UserQueryModel>>), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
//    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.GetUser}")]
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapGet("api/user", async (ISender sender) =>
//        {
//            var result = await sender.Send(new GetUserQuery());
//            return Results.Ok(result.Value);
//        })
//        .WithName("GetUser")
//        .WithOpenApi(x => new OpenApiOperation(x)
//        {
//            Summary = "Buscar todos usuarios",
//            Description = "Buscar todos usuarios",
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
