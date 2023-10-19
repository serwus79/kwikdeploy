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

    public async Task<ResultWithId<string>> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new InternalServerErrorException(result.Errors.ToDictionary(x=>x.Code, x=>x.Description));
        }

        return ResultWithId<string>.Success(user!.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User", userId);
        }

        return await DeleteUserAsync(user);
    }
    
    public async Task<bool> IsUniqueUserName(string userName, string? userId, CancellationToken cancellationToken = default)
    {
        var normalizedUserName = _userManager.NormalizeName(userName);

        return !await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName && x.Id != userId,
        cancellationToken);
    }
    
    public async Task<bool> IsUniqueEmail(string email, string? userId, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = _userManager.NormalizeEmail(email);
        
        return !await _userManager.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail && x.Id != userId,
        cancellationToken);
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new InternalServerErrorException(result.Errors.ToDictionary(x => x.Code, x => x.Description));
        }

        return Result.Success();
    }
}
