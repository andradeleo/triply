using Communication.Request;
using FluentValidation;

namespace Application.UseCases.Authentication
{
    public  class LoginValidator : AbstractValidator<RequestLogin>
    {
        public LoginValidator()
        {
            RuleFor(data => data.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório")
                .EmailAddress()
                .When(data => string.IsNullOrWhiteSpace(data.Email) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage("Email inválido");
        }
    }
}
