using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI
{
    interface IFetcher
    {
        string ProviderName { get; }
        Task<ForecastGeneralized> GetDataAsync(string uniqueCityId, string cityName);
    }
}
