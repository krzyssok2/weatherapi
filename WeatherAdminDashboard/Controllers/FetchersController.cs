using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WeatherAdminDashboard.Models;
using WeatherForecastAPI;
using WeatherForecastAPI.Worker;
namespace WeatherAdminDashboard.Controllers
{
    public class FetchersController : Controller
    {
        public WeatherContext _context;
        public FetchersController(WeatherContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<string> cities = GetAllCities();
            List<string> providers = GetAllProviders();
            return View(new FetchersViewModel { Cities=cities, Providers=providers });
        }
        [BindProperty(SupportsGet = true)]
        public string City { get; set; }
        public IActionResult Forecast(string city, string provider)
        {
            return View(new CityViewModel
            {
                CityName = city ,
                ProviderName=provider
            });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private List<string> GetAllCities()
        {
            return _context.Cities.Select(i => i.Name).ToList();
        }

        private List<string> GetAllProviders()
        {
            return _context.Providers.Select(i => i.ProviderName).ToList();
        }
        //public IActionResult OnPost(string city)
        //{
        //    return RedirectToAction("Forecast", "Fetchers", );
        //}
    }
}
