using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Application.Users.Queries;

public class UserGet : IRequest<UserDto>
{
    [FromRoute]
    public string Id { get; set; } = default!;
}

public class UserGetHandler : IRequestHandler<UserGet, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserGetHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(UserGet request, CancellationToken cancellationToken)
    {
        UserDto? userDto = await _userManager.Users.Select(x => new UserDto
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email,
            EmailConfirmed = x.EmailConfirmed,
            LockoutEnabled = x.LockoutEnabled,
            LockoutEnd = x.LockoutEnd,
            AccessFailedCount = x.AccessFailedCount
        }).FirstOrDefaultAsync(predicate: x => x.Id == request.Id, cancellationToken);

        if (userDto is null)
        {
            throw new NotFoundException(nameof(UserDto), request.Id);
        }

        return userDto;
    }
}