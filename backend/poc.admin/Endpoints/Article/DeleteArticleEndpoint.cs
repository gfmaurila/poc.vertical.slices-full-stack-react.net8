﻿using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using poc.admin.Feature.Articles.DeleteArticle;
using poc.core.api.net8.API.Models;
using System.Net.Mime;

namespace poc.vertical.slices.net8.Endpoints.Article;

public class DeleteArticleEndpoint : ICarterModule
{
    /// <summary>
    /// Deleta o registro pelo Id.
    /// </summary>
    /// <param name="id">O ID do artigo a ser deletado.</param>
    /// <param name="sender"></param>
    /// <response code="200">Retorna a resposta com a mensagem de sucesso.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhum registro é encontrado pelo Id fornecido.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpDelete("api/articles/{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = $"{RoleUserAuthConstants.User}, {RoleUserAuthConstants.DeleteUser}")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/articles/{id}", async (Guid id, ISender sender) =>
        {
            var query = new DeleteArticleCommand { Id = id };
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        });
    }
}