using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI
{
    interface IFetcher
    {
        string ProviderName { get; }
        Task<ForecastGeneralized> GetData(string uniqueCityId, string cityName);
    }
}
