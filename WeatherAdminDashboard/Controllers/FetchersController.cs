using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherAdminDashboard.Models;
namespace WeatherAdminDashboard.Controllers
{
    public class FetchersController : Controller
    {

        public FetchersController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        [BindProperty(SupportsGet = true)]
        public string City { get; set; }
        public IActionResult Forecast(string city, string provider)
        {
            return View(new CityViewModel { CityName = city , ProviderName=provider});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        //public IActionResult OnPost(string city)
        //{
        //    return RedirectToAction("Forecast", "Fetchers", );
        //}
    }
}
