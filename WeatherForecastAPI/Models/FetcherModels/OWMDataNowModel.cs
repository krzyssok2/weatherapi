namespace WeatherForecastAPI.Models
{
    public class Main
    {
        public double temp { get; set; }
        //public double feels_like { get; set; }
        public int temp_min { get; set; }
        public double temp_max { get; set; }
        //public int pressure { get; set; }
        //public int humidity { get; set; }
    }

    public class OWMNowRootObject
    {
        //public Coord coord { get; set; }
        //public List<Weather> weather { get; set; }
        //public string @base { get; set; }
        public Main main { get; set; }
        //public int visibility { get; set; }
        //public Wind wind { get; set; }
        //public Clouds clouds { get; set; }
        public int dt { get; set; }
        //public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        //public int cod { get; set; }
    }
}
