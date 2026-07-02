using Communication.Resources;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Application.UseCases.Authentication.Register
{
    public partial class PasswordValidator<T> : PropertyValidator<T, string>
    {
        private const string ERROR_MESSAGE_KEY = "ErrorMessage";

        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"{{{ERROR_MESSAGE_KEY}}}";
        }

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            var isValid =
                password.Length >= 8 &&
                UpperCaseLetter().IsMatch(password) &&
                LowerCaseLetter().IsMatch(password) &&
                Number().IsMatch(password) &&
                SpecialSymbol().IsMatch(password);

            if (!isValid)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceMessages.INVALID_PASSWORD);
            }

            return isValid;
        }

        [GeneratedRegex(@"[A-Z]")]
        private static partial Regex UpperCaseLetter();

        [GeneratedRegex(@"[a-z]")]
        private static partial Regex LowerCaseLetter();

        [GeneratedRegex(@"\d")]
        private static partial Regex Number();

        [GeneratedRegex(@"[!@#$%^&*()\-_+=\[\]{}|;:,./?~]")]
        private static partial Regex SpecialSymbol();
    }
}
