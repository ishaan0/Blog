using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Articles.GetArticles;
using Blog.Application.Articles.GetById;
using Blog.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ISender mediator, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles(
        [FromQuery] ArticlesGetRequest articlesGetRequest,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetArticlesQuery>(articlesGetRequest);

        var articles = await mediator.Send(query, cancellationToken);

        return Ok(articles.Items);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> CreateArticle(
        [FromBody] ArticleCreationRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateArticleCommand>(request);

        var article = await mediator.Send(command, cancellationToken);
        return CreatedAtRoute("GetArticleById", new { id = article.Id },
            new ApiResponse<ArticleResponse>(
                true,
                StatusCodes.Status201Created,
                "Article created",
                new List<ArticleResponse>() { article }));
    }

    [HttpGet("{id:guid}", Name = "GetArticleById")]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetArticleByIdQuery() { Id = id };

        var article = await mediator.Send(query, cancellationToken);

        return Ok(new ApiResponse<ArticleResponse>(
            true,
            StatusCodes.Status200OK,
            "Article found",
            new List<ArticleResponse>() { article }));
    }
}
