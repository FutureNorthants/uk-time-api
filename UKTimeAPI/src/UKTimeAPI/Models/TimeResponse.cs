namespace UKTimeAPI.Models
{
    public class TimeResponse
    {
        public string CurrentTime { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public bool IsDaylightSaving { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ErrorResponse
    {
        public string Error { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}