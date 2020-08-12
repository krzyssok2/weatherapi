using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI
{
   public interface IFetcher
    {
        string ProviderName { get; }
        Task<ForecastGeneralized> GetData(string uniqueCityId, string cityName);
    }
}
