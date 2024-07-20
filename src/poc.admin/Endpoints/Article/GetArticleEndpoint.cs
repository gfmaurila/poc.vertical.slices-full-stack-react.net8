using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using poc.admin.Feature.Articles;
using poc.admin.Feature.Articles.GetArticle;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.Article;

public class GetArticleEndpoint : ICarterModule
{
    /// <summary>
    /// Obtém uma lista com todos os registros.
    /// </summary>
    /// <param name="sender">O ISender para enviar comandos de consulta.</param>
    /// <response code="200">Retorna a lista de registros.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpGet("api/articles")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<List<ArticleResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.GetUser}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/articles", async (ISender sender) =>
        {
            var result = await sender.Send(new GetArticleQuery());
            return Results.Ok(result.Value);
        });
    }
}
