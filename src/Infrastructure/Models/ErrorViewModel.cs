namespace Infrastructure.Models;

public class ErrorViewModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string ErrorLocation { get; set; }

    public ErrorViewModel(string errorTypeMessage, int statusCode, Exception e)
    {
        Message = errorTypeMessage + ": " + e.Message;
        StatusCode = statusCode;
        ErrorLocation = (e.StackTrace != null ? e.StackTrace.Split(" in ")[0].Replace("  at ", "") : "").Trim();
    }
}
