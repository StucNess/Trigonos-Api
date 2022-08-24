namespace TrigonosEnergyWebAPI.Errors
{
    public class CodeErrorExeption : CodeErrorResponse
    {
        public CodeErrorExeption(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}
