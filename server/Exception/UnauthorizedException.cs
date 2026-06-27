using System.Net;

namespace Exception
{
    public class UnauthorizedException(string errorMessages) : ExceptionBase(string.Empty)
    {
        private readonly List<string> _errors = [errorMessages];

        public override int StatusCode => (int)HttpStatusCode.Unauthorized;

        public override List<string> GetErrors()
        {
            return _errors;
        }
    }
}
