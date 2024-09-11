using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Api.Helpers;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Articles.GetArticles;
using Blog.Application.Articles.GetById;
using Blog.Application.Dtos.Articles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ISender mediator, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles(
        [FromQuery] GetArticlesDto getArticleDto,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetArticlesQuery>(getArticleDto);

        var articles = await mediator.Send(query, cancellationToken);

        return Ok(articles.Items);
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(
        [FromBody] ArticleCreationRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateArticleCommand>(request);

        var articleId = await mediator.Send(command, cancellationToken);

        return ApiResponseHelper.Created(nameof(CreateArticle), articleId);
    }

    [HttpGet("{id:guid}", Name = "GetArticleById")]
    public async Task<IActionResult> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetArticleByIdQuery() { Id = id };

        var article = await mediator.Send(query, cancellationToken);

        return ApiResponseHelper.Success(article);
    }


}
