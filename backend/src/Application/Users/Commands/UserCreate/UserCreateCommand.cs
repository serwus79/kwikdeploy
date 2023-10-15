using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Domain.Exceptions;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    private readonly UserManager<ApplicationUser> _userManager;

    public UserCreateCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ResultWithId<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        string normalizedUserName = _userManager.NormalizeName(request.UserName);
        if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.UserName), "Another user with the same name already exists.")
            });
        }

        if (request.Email is not null)
        {
            var normalizedEmail = _userManager.NormalizeEmail(request.Email);
            if (await _userManager.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new(nameof(request.Email), "Another user with the same e-mail already exists.")
                });
            }
        }

        IdentityResult result = await _userManager.CreateAsync(new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            NormalizedUserName = normalizedUserName,
            NormalizedEmail = _userManager.NormalizeEmail(request.Email)
        });

        if (!result.Succeeded)
        {
            IEnumerable<string> err = result.Errors.Select(x => x.Code);
            throw new UserCreateErrorException();
        }

        ApplicationUser? user = await _userManager.FindByNameAsync(normalizedUserName);
        return ResultWithId<string>.Success(user!.Id);
    }
}