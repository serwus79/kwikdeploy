using FluentValidation.Results;
using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Application.Users.Commands.UserSetEmail;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Commands.UserSetName;

public class UserSetNameCommand:IRequest<Result>
{
    public string Id { get; set; }
    public string UserName { get; set; }
}

public class UserSetNameCommandHandler : IRequestHandler<UserSetNameCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSetNameCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Result> Handle(UserSetNameCommand request, CancellationToken cancellationToken)
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

        if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName && x.Id != request.Id,
                cancellationToken))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new(nameof(request.UserName), "Another user with the same user name already exists.")
            });
        }

        
        
    }
}