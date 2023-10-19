using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Application.Common.Models;
using MediatR;

namespace KwikDeploy.Application.Users.Commands.UserCreate;

public class UserCreateCommand : IRequest<ResultWithId<string>>
{
    public string UserName { get; set; } = default!;
    public string? Email { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, ResultWithId<string>>
{
    private readonly IIdentityService _identityService;

    public UserCreateCommandHandler(IIdentityService identityService)
    {

        _identityService = identityService;
    }

    public async Task<ResultWithId<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        if (!await _identityService.IsUniqueUserName(request.UserName, null, cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.UserName), "Another user with the same name already exists.")
            });
        }

        if (request.Email is not null && !await _identityService.IsUniqueEmail(request.Email, null, cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.Email), "Another user with the same e-mail already exists.")
            });
        }

        return await _identityService.CreateUserAsync(request.UserName, request.Password);
    }
}