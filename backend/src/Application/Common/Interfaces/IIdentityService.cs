using KwikDeploy.Application.Common.Models;

namespace KwikDeploy.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<ResultWithId<string>> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);

    Task<bool> IsUniqueUserName(string userName, string? userId, CancellationToken cancellationToken = default);
    
    Task<bool> IsUniqueEmail(string email, string? userId, CancellationToken cancellationToken = default);
}
