using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Articles.GetArticles;
using Blog.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(ISender mediator, IMapper mapper) : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ArticleResponse>>> Articles(
        [FromQuery] ArticlesGetRequest articlesGetRequest,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetArticlesQuery>(articlesGetRequest);

        var articles = await mediator.Send(query, cancellationToken);

        return Ok(articles.Items);
    }

    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<string>>> CreateArticle(
        [FromBody] ArticleCreationRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateArticleCommand>(request);

        var response = await mediator.Send(command, cancellationToken);

        return Created("testing url",
            new ApiResponse<string>(
                true,
                StatusCodes.Status201Created,
                response.ArticleId.ToString(),
                null
           ));
    }

}
