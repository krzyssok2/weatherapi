using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public enum EnumErrors
    {
        CantBeEmpty = 0,
        MustBeEmail = 1,
        CantBeNull = 2,
        EnumDoesntExist=3
    }
    public class ErrorModel
    {
        public string FieldName { get; set; }
        public List<EnumErrors> ErrorType { get; set; }
    }
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
