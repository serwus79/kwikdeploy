using KwikDeploy.Api.Controllers;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Application.Users.Commands.UserCreate;
using KwikDeploy.Application.Users.Commands.UserDelete;
using KwikDeploy.Application.Users.Commands.UserSetEmail;
using KwikDeploy.Application.Users.Commands.UserSetName;
using KwikDeploy.Application.Users.Queries.UserGet;
using KwikDeploy.Application.Users.Queries.UserGetList;
using KwikDeploy.Application.Users.Queries.UserUniqueEmailQuery;
using KwikDeploy.Application.Users.Queries.UserUniqueNameQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Users.Queries.UserGet;

namespace Api.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserHeadDto>>> GetList([FromQuery] UserGetListQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById([FromRoute] string id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new UserGetQuery { Id = id }, cancellationToken);
    }

    [HttpGet("uniqueUserName")]
    public async Task<ActionResult<bool>> IsUniqueUserName([FromQuery] UserUniqueUserNameQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpGet("uniqueEmail")]
    public async Task<ActionResult<bool>> IsUniqueEmail([FromQuery] UserUniqueEmailQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<ResultWithId<string>>> Create(UserCreateCommand command,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }

    [HttpDelete]
    public async Task<ActionResult<Result>> Delete(UserDeleteCommand command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }

    [HttpPut("{id}/email")]
    public async Task<ActionResult<Result>> SetEmail(
        [FromRoute] string id,
        [FromBody] UserSetEmailDto data,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new UserSetEmailCommand { Id = id, Email = data.Email }, cancellationToken);
    }

    [HttpPut("{id}/userName")]
    public async Task<ActionResult<Result>> SetUserName(
        [FromRoute] string id,
        [FromBody] UserSetUserNameDto data,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new UserSetUserNameCommand { Id = id, UserName = data.UserName }, cancellationToken);
    }
}