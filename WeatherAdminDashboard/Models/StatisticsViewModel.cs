using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAdminDashboard.Models
{
    public class StatisticsViewModel
    {
        public int HowManyFetchedToday { get; set; }
        public int HowManyFetchedThisWeek { get; set; }
        public int HowManyFetchedThisMonth { get; set; }
        public int HowManyFetchedAllTime { get; set; }
        public ProvidersFetched ProvidersFetched { get; set; }
    }
    public class ProvidersFetched
    {
        public List<Fetched> ProviderFetched { get; set; }
    }
    public class Fetched
    {
        public string Providers { get; set; }
        public int HowMany { get; set; }
    }
}
