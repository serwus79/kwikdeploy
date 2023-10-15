using FluentValidation;

namespace KwikDeploy.Application.Users.Queries.UserUniqueEmailQuery;

public class UserUniqueEmailValidator: AbstractValidator<UserUniqueEmailQuery>
{
    public UserUniqueEmailValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();
    }
}