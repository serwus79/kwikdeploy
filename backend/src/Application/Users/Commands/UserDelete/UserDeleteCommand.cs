using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Application.Common.Models;
using MediatR;

namespace KwikDeploy.Application.Users.Commands.UserDelete;

public class UserDeleteCommand : IRequest<Result>
{
    public string Id { get; set; } = default!;
}

public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, Result>
{
    private readonly IIdentityService _identityService;

    public UserDeleteCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.DeleteUserAsync(request.Id, cancellationToken);
    }
}