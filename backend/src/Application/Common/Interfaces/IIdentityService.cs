using KwikDeploy.Application.Common.Models;

namespace KwikDeploy.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);
    
    Task<Result<string>> CreateUserAsync(string userName, string password, CancellationToken cancellationToken = default);

    Task<bool> IsUniqueNameAsync(string userName, string? userId = null, CancellationToken cancellationToken = default);

    Task<bool> IsUserExist(string userId, CancellationToken cancellationToken = default);

    Task SetUserNameAsync(string userId, string userName, CancellationToken cancellationToken = default);

    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
}
