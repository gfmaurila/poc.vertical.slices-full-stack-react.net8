﻿using API.Admin.Feature.Users.CreateUser;
using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using poc.core.api.net8.API.Models;

namespace poc.vertical.slices.net8.Endpoints.User;

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/User", HandleCreateUser)
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(StatusCodes.Status200OK)
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
        var result = await sender.Send(command);
        if (!result.Success)
            return Results.BadRequest(result);
        return Results.Ok(result);
    }
}