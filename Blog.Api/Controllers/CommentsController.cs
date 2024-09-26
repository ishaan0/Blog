using AutoMapper;
using Blog.Api.Helpers;
using Blog.Application.Comments.CreateComment;
using Blog.Application.Dtos.Comments;
using Blog.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(
    ISender mediator,
    IMapper mapper,
    IValidator<CreateCommentDto> createCommentDtoValidator
    ) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetComments(CreateCommentDto createCommentDto)
    {
        var validatorResult = await createCommentDtoValidator.ValidateAsync(createCommentDto);
        if (validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();
            return ApiResponseHelper.BadRequest("Bad request", errorMessages);
        }

        var command = mapper.Map<CreateCommentCommand>(createCommentDto);

        Guid commentId = await mediator.Send(command);

        return ApiResponseHelper.Created(nameof(GetComments), commentId, "Comment created successfully");
    }
}
