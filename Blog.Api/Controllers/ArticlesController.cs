using AutoMapper;
using Blog.Api.Helpers;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Articles.GetArticles;
using Blog.Application.Articles.GetById;
using Blog.Application.Articles.UpdateArticle;
using Blog.Application.Dtos.Articles;
using Blog.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(
    ISender mediator,
    IMapper mapper,
    IValidator<CreateArticleDto> createArticleDtoValidator,
    IValidator<UpdateArticleDto> updateArticleDtoValidator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<ArticleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticles(
        [FromQuery] GetArticlesDto getArticleDto,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetArticlesQuery>(getArticleDto);

        var articles = await mediator.Send(query, cancellationToken);

        return ApiResponseHelper.Success(articles, "Articles fetched successfully");
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(
        [FromBody] CreateArticleDto createArticleDto,
        CancellationToken cancellationToken)
    {
        var validatorResult = await createArticleDtoValidator.ValidateAsync(createArticleDto);

        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            return ApiResponseHelper.BadRequest("Bad request", errorMessages);
        }

        var command = mapper.Map<CreateArticleCommand>(createArticleDto);

        var articleId = await mediator.Send(command, cancellationToken);

        return ApiResponseHelper.Created(nameof(CreateArticle), articleId, "Article created successfully");
    }

    [HttpGet("{id:guid}", Name = "GetArticleById")]
    public async Task<IActionResult> GetArticleById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetArticleByIdQuery() { Id = id };

        var article = await mediator.Send(query, cancellationToken);

        return ApiResponseHelper.Success(article, "article fetched successfully");
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateArticleById(
        Guid id,
        UpdateArticleDto updateArticleDto,
        CancellationToken cancellationToken)
    {
        var validatorResult = await updateArticleDtoValidator.ValidateAsync(updateArticleDto);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            return ApiResponseHelper.BadRequest("Bad request", errorMessages);
        }

        var command = mapper.Map<UpdateArticleCommand>(updateArticleDto);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return ApiResponseHelper.NoContent();
    }


}
