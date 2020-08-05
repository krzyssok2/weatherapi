using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Services
{
    public class SettingsServices
    {
        public Settings GetAllSettings(List<UserSettings> userSettings, string userName)
        {
            var result= userSettings
                .Where(i => i.User == userName)
                .Select(i => new Settings
                {
                    PreferedUnit = i.Units,
                    FavoriteCities = i.FavoriteCities.Select(j => new PreferedCities
                    {
                        CityId = j.City.Id,
                        CityName = j.City.Name,
                        Country = j.City.Country
                    }).ToList()
                }).First();

            return result;
        }

        public List<PreferedCities> GetPreferedCities(List<FavoriteCities> favoriteCities, string userName)
        {
            var result = favoriteCities
                .Where(i => i.User.User == userName)
                .Select(i => new PreferedCities
                {
                    CityId = i.City.Id,
                    CityName = i.City.Name,
                    Country = i.City.Country
                }).ToList();

            return result;
        }
    }
}
