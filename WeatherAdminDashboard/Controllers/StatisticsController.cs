using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherAdminDashboard.Models;
using WeatherForecastAPI;

namespace WeatherAdminDashboard.Controllers
{
    public class StatisticsController : Controller
    {
        public WeatherContext _context;
        public StatisticsController(WeatherContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            int howManyFetchedToday = GetHowManyFetchedToday();
            int howManyFetchedThisWeek = GetHowManyFetchedThisWeek();
            int howManyFetchedThisMonth = GetHowManyFetchedThisMonth();
            int howManyFetchedOverall = _context.Forecasts.Count();

            var distinct = _context.Forecasts.Select(x => x.Provider).Distinct().ToList();
            var providersFetched = new ProvidersFetched
            {
                ProviderFetched = new List<Fetched>()
            };
            foreach (var item in distinct)
            {
                var addition = new Fetched
                {
                    Providers = item,
                    HowMany = _context.Forecasts.Where(x => x.Provider == item).Count()
                };
                providersFetched.ProviderFetched.Add(addition);
            }
            return View(new StatisticsViewModel
            {
                HowManyFetchedToday = howManyFetchedToday,
                HowManyFetchedThisWeek = howManyFetchedThisWeek,
                HowManyFetchedThisMonth = howManyFetchedThisMonth,
                ProvidersFetched=providersFetched,
                HowManyFetchedAllTime=howManyFetchedOverall
            });
        }
        private int GetHowManyFetchedToday()
        {
            return _context.Forecasts.Where(i => i.CreatedDate.Date == DateTime.Now.Date).Count();
        }

        private int GetHowManyFetchedThisMonth()
        {
            return _context.Forecasts.Where(i => i.CreatedDate.Month == DateTime.Now.Month&& i.CreatedDate.Year==DateTime.Now.Year).Count();
        }
        private int GetHowManyFetchedThisWeek()
        {
            DateTime start = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime end = start.AddDays(7);
            return _context.Forecasts.Where(i => i.CreatedDate.Date >= start && i.CreatedDate.Date < end).Count();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
