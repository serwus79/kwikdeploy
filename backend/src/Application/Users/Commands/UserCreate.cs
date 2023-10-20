using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Domain.Exceptions;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Commands;

public record UserCreate : IRequest<Result<string>>
{
    [FromBody]
    public UserCreateBody Body { get; init; } = null!;
}

public record UserCreateBody
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

public class UserCreateHandler : IRequestHandler<UserCreate, Result<string>>
{
    private readonly IIdentityService _identityService;

    public UserCreateHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<string>> Handle(UserCreate request, CancellationToken cancellationToken)
    {
        if (!await _identityService.IsUniqueNameAsync(request.Body.UserName, cancellationToken: cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.Body.UserName), "Another user with the same name already exists.")
            });
        }
        
        return await _identityService.CreateUserAsync(request.Body.UserName, request.Body.Password, cancellationToken);
    }
}