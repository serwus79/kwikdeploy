using KwikDeploy.Application.Common.Exceptions;
using KwikDeploy.Application.Common.Interfaces;
using KwikDeploy.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KwikDeploy.Application.Users.Commands;

public class UserDelete : IRequest
{
    [FromRoute]
    public string Id { get; set; } = default!;
}

public class UserDeleteHandler : IRequestHandler<UserDelete>
{
    private readonly IIdentityService _identityService;

    public UserDeleteHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(UserDelete request, CancellationToken cancellationToken)
    {
        await _identityService.DeleteUserAsync(request.Id, cancellationToken);
    }
}