namespace WeatherForecastAPI.Models
{
    public enum HandlingErrors
    {
        TimeSpanTooLong,
        NoDataFound,
    }
    public class ErrorHandlingModel
    {
        public HandlingErrors Error { get; set; }
    }
}
