using KwikDeploy.Api.Controllers;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Application.Users.Commands;
using KwikDeploy.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserHeadDto>>> GetList(UserGetList query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<UserDto>> GetById(UserGet query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("uniqueUserName")]
    public async Task<ActionResult<Result<bool>>> IsUniqueUserName(UserUniqueUserName query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("uniqueEmail")]
    public async Task<ActionResult<Result<bool>>> IsUniqueEmail(UserUniqueEmail query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<Result<string>>> Create(UserCreate command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(UserDelete command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{Id}/userName")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetUserName(UserSetUserName command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}