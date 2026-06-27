namespace Exception
{
    public abstract class ExceptionBase(string message) : SystemException(message)
    {
        public abstract int StatusCode { get; }

        public abstract List<string> GetErrors();
    }
}
