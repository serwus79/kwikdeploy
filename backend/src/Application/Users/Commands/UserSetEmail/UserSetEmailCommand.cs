using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Commands.UserSetEmail;

public class UserSetEmailCommand : IRequest<Result>
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class UserSetEmailCommandHandler : IRequestHandler<UserSetEmailCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSetEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(UserSetEmailCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException(nameof(UserSetEmailCommand.Id), request.Id);
        }

        string normalizedEmail = _userManager.NormalizeEmail(request.Email);
        if (user.NormalizedEmail.Equals(normalizedEmail))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.Email), "The new email address must be different from the current one.")
            });
        }

        if (await _userManager.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.Email), "Another user with the same e-mail already exists.")
            });
        }

        IdentityResult result = await _userManager.SetEmailAsync(user, request.Email);
        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(x => $"{x.Code}: ${x.Description}"));
        }

        return Result.Success();
    }
}