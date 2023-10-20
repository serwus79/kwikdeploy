using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KwikDeploy.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }
    public async Task<Result<string>> CreateUserAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }

        return new Result<string>(user.Id);
    }

    public async Task<bool> IsUniqueNameAsync(string userName, string? userId = null, CancellationToken cancellationToken = default)
    {
        var normalizedUserName = _userManager.NormalizeName(userName);
        return !await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName && x.Id != userId, cancellationToken: cancellationToken);
    }
    public async Task<bool> IsUserExist(string userId, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.AnyAsync(x => x.Id == userId, cancellationToken: cancellationToken);
    }

    public async Task SetUserNameAsync(string userId, string userName, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User", userId);
        }
        
        var result = await _userManager.SetUserNameAsync(user, userName);
        
        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }
    }
    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User", userId);
        }

        await DeleteUserAsync(user);
    }

    public async Task DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }
    }
}
