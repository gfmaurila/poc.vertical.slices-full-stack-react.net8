using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using poc.admin.Feature.Articles;
using poc.admin.Feature.Articles.CreateArticle;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.Article;
public class CreateArticleEndpoint : ICarterModule
{
    /// <summary>
    /// Cria um artigo.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="sender"></param>
    /// <response code="200">Retorna o artigo criado.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpPost("api/articles")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)] // Adapte o tipo conforme necessário
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/articles", async (CreateArticleRequest request, ISender sender) =>
        {

            var command = request.Adapt<CreateArticleCommand>();

            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        });
    }
}

