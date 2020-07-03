using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 
    public class MainF
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        //public int pressure { get; set; }
        //public int sea_level { get; set; }
        //public int grnd_level { get; set; }
        //public int humidity { get; set; }
        //public double temp_kf { get; set; }

    }

    public class WeatherF
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

    }

    public class CloudsF
    {
        public double all { get; set; }

    }

    public class WindF
    {
        public double speed { get; set; }
        public int deg { get; set; }

    }

    public class SysF
    {
        public string pod { get; set; }

    }

    public class RainF
    {
        public double rain { get; set; }

}

public class ListF
{
    //public int dt { get; set; }
    public MainF main { get; set; }
    //public List<WeatherF> weather { get; set; }
    //public CloudsF clouds { get; set; }
    //public WindF wind { get; set; }
    //public SysF sys { get; set; }
    public DateTime dt_txt { get; set; }
    //public RainF rain { get; set; }

}

public class CoordF
{
    public double lat { get; set; }
    public double lon { get; set; }

}

public class CityF
{
    public int id { get; set; }
    public string name { get; set; }
    //public CoordF coord { get; set; }
    public string country { get; set; }
    //public int population { get; set; }
    //public int timezone { get; set; }
    //public int sunrise { get; set; }
    //public int sunset { get; set; }

}

public class OWMForecastRootObject
{
    //public double cod { get; set; }
    //public int message { get; set; }
    //public int cnt { get; set; }
    public List<ListF> list { get; set; }
    public CityF city { get; set; }

}


}
