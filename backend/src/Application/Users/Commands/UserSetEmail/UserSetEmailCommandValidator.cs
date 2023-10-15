using FluentValidation;

namespace KwikDeploy.Application.Users.Commands.UserSetEmail;

public class UserSetEmailCommandValidator : AbstractValidator<UserSetEmailCommand>
{
    public UserSetEmailCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();
        RuleFor(v => v.Id).NotEmpty();
    }
}