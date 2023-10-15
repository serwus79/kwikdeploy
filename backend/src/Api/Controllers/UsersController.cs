using KwikDeploy.Api.Controllers;
using KwikDeploy.Application.Common.Models;
using KwikDeploy.Application.Users.Commands.UserCreate;
using KwikDeploy.Application.Users.Commands.UserDelete;
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

    [HttpGet("uniquename")]
    public async Task<ActionResult<bool>> IsUniqueUserName([FromQuery] UserUniqueNameQuery query,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }
    
    [HttpGet("uniqueemail")]
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
}