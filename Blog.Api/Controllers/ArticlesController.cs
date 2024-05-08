using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.GetArticles;
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
}
