using FluentValidation;

namespace KwikDeploy.Application.Users.Commands;

public class UserCreateValidator : AbstractValidator<UserCreate>
{
    public UserCreateValidator()
    {
        RuleFor(v => v.Body.UserName).MaximumLength(255).NotEmpty().EmailAddress();
        RuleFor(v => v.Body.Password).MaximumLength(255).NotEmpty();
        RuleFor(v => v.Body.ConfirmPassword)
            .MaximumLength(255).NotEmpty().Equal(v => v.Body.Password).WithMessage("ConfirmPassword must be equal to Password.");
    }
}