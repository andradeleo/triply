using System.Net;

namespace Exception
{
    public class ErrorOnValidationException(List<string> errorMessages) : ExceptionBase(String.Empty)
    {
        private readonly List<string> _errors = errorMessages;

        public override int StatusCode => (int)HttpStatusCode.BadRequest;

        public override List<string> GetErrors()
        {
            return _errors;
        }
    }
}
