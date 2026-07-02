using Communication.Request;
using Communication.Resources;
using FluentValidation;

namespace Application.UseCases.Authentication.Register
{
    public class RegisterValidator : AbstractValidator<RequestRegister>
    {
        public RegisterValidator()
        {
            RuleFor(data => data.Name)
                .NotEmpty()
                .WithMessage(ResourceMessages.NAME_REQUIRED)
                .MaximumLength(100)
                .WithMessage(ResourceMessages.NAME_TOO_LONG)
                .Matches(@"^[\p{L}\p{M} '-]+$")
                .When(data => string.IsNullOrWhiteSpace(data.Name) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage(ResourceMessages.NAME_INVALID);

            RuleFor(data => data.Email)
                .NotEmpty()
                .WithMessage(ResourceMessages.EMAIL_REQUIRED)
                .EmailAddress()
                .When(data => string.IsNullOrWhiteSpace(data.Email) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage(ResourceMessages.INVALID_EMAIL);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ResourceMessages.PASSWORD_REQUIRED)
                .SetValidator(new PasswordValidator<RequestRegister>());
        }
    }
}
