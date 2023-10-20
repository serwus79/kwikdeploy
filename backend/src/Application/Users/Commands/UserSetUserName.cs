using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Commands;

public record UserSetUserName : IRequest
{
    [FromRoute]
    public string Id { get; init; } = default!;

    [FromBody]
    public UserSetUserNameBody Body { get; init; } = null!;
}

public record UserSetUserNameBody
{
    public string UserName { get; init; } = default!;
}

public class UserSetUserNameHandler : IRequestHandler<UserSetUserName>
{
    private readonly IIdentityService _identityService;


    public UserSetUserNameHandler(IIdentityService _identityService)
    {
        this._identityService = _identityService;
    }

    public async Task Handle(UserSetUserName request, CancellationToken cancellationToken)
    {
        if (!await _identityService.IsUserExist(request.Id, cancellationToken))
        {
            throw new NotFoundException(nameof(request.Id), request.Id);
        }

        if (!await _identityService.IsUniqueNameAsync(request.Body.UserName, cancellationToken: cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.Body.UserName), "The new user name must be different from the current one or user with the same user name already exists.")
            });         
        }

        await _identityService.SetUserNameAsync(request.Id, request.Body.UserName, cancellationToken);
    }
}