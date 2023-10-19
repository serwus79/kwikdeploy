using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Application.Users.Commands.UserSetEmail;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Commands.UserSetName;

public class UserSetUserNameCommand:IRequest<Result>
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
}

public class UserSetUserNameCommandHandler : IRequestHandler<UserSetUserNameCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public UserSetUserNameCommandHandler(UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }
    public async Task<Result> Handle(UserSetUserNameCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException(nameof(UserSetEmailCommand.Id), request.Id);
        }

        var normalizedUserName = _userManager.NormalizeName(request.UserName);
        if (user.NormalizedUserName!.Equals(normalizedUserName))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.UserName), "The new user name must be different from the current one.")
            });
        }

        if (!await _identityService.IsUniqueUserName(request.UserName, request.Id, cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.UserName), "Another user with the same user name already exists.")
            });
        }

        var result = await _userManager.SetUserNameAsync(user, request.UserName);
        
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(x => x.Code, x => x.Description);
            throw new InternalServerErrorException(errors);
        }

        return Result.Success();
    }
}