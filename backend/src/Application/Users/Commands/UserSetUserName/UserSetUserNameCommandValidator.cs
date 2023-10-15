using FluentValidation;

namespace KwikDeploy.Application.Users.Commands.UserSetName;

public class UserSetUserNameCommandValidator:AbstractValidator<UserSetUserNameCommand>
{
    public UserSetUserNameCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.UserName).NotEmpty();
    }
}