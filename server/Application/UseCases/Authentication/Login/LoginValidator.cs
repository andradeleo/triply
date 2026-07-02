using Communication.Request;
using Communication.Resources;
using FluentValidation;

namespace Application.UseCases.Authentication.Login
{
    public  class LoginValidator : AbstractValidator<RequestLogin>
    {
        public LoginValidator()
        {
            RuleFor(data => data.Email)
                .NotEmpty()
                .WithMessage(ResourceMessages.EMAIL_REQUIRED)
                .EmailAddress()
                .When(data => string.IsNullOrWhiteSpace(data.Email) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage(ResourceMessages.INVALID_EMAIL);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Must(password => !string.IsNullOrWhiteSpace(password))
                .WithMessage(ResourceMessages.PASSWORD_REQUIRED);
        }
    }
}
