using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using poc.admin.Feature.Articles.UpdateArticle;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.Article;
public class UpdateArticleEndpoint : ICarterModule
{

    /// <summary>
    /// Atualiza um registro existente.
    /// </summary>
    /// <param name="request">Dados do artigo para atualização.</param>
    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpPut("api/articles")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.PutUser}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/articles", async (UpdateArticleRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateArticleCommand>();
            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
        .WithName("PutArticles")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Atualizar artigos",
            Description = "Atualizar artigos",
            Tags = new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = "Artigos"
                }
            }
        });
    }
}

