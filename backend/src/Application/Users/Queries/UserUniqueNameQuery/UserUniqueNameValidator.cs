using FluentValidation;

namespace KwikDeploy.Application.Users.Queries.UserUniqueNameQuery;

public class UserUniqueNameValidator: AbstractValidator<UserUniqueUserNameQuery>
{
    public UserUniqueNameValidator()
    {
        RuleFor(v => v.UserName).NotEmpty();
    }
}