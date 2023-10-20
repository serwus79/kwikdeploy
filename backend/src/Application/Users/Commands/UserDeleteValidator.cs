using FluentValidation;

namespace KwikDeploy.Application.Users.Commands;

public class UserDeleteValidator : AbstractValidator<UserDelete>
{
    public UserDeleteValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}