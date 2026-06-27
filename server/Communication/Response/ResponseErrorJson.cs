namespace Communication.Response
{
    public class ResponseErrorJson
    {
        public List<string> ErrorMessages { get; set; }

        public ResponseErrorJson(string ErrorMessages)
        {
            this.ErrorMessages = [ErrorMessages];
        }

        public ResponseErrorJson(List<string> ErrorMessages)
        {
            this.ErrorMessages = ErrorMessages;
        }
    }
}
