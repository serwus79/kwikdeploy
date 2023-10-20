using FluentValidation;

namespace KwikDeploy.Application.Users.Commands;

public class UserSetUserNameValidator : AbstractValidator<UserSetUserName>
{
    public UserSetUserNameValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Body.UserName).NotEmpty().EmailAddress();
    }
}