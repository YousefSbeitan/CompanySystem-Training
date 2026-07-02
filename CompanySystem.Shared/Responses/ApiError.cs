namespace CompanySystem.Shared.Responses;

public class ApiError
{
    public DateTime Timestamp { get; set; }

    public int Status { get; set; }

    public string Error { get; set; }

    public string Message { get; set; }

    public string Path { get; set; }

    public ApiError(
        int status,
        string error,
        string message,
        string path)
    {
        Timestamp = DateTime.UtcNow;
        Status = status;
        Error = error;
        Message = message;
        Path = path;
    }
}