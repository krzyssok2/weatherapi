using System.Collections.Generic;

public class Weather
{
    public int id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }

}

    public class Hourly
{
    public int dt { get; set; }
    public double temp { get; set; }
    //public double feels_like { get; set; }
    //public int pressure { get; set; }
    //public int humidity { get; set; }
    //public double dew_point { get; set; }
   // public int clouds { get; set; }
    //public double wind_speed { get; set; }
    //public int wind_deg { get; set; }
    //public List<Weather2> weather { get; set; }
    //public Rain rain { get; set; }
}

public class OWMOneCallRootObject
{
    //public double lat { get; set; }
    //public double lon { get; set; }
   // public string timezone { get; set; }
    //public int timezone_offset { get; set; }
    //public Current current { get; set; }
    public List<Hourly> hourly { get; set; }

}

