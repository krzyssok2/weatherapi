﻿using System.Collections.Generic;

namespace WeatherForecastAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 
   
    public class Item
    {
        //public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string pubDate { get; set; }
        //public Guid guid { get; set; }
        //public string dcdate { get; set; }
        //public string georsspoint { get; set; }
    }

    public class Channel
    {
        public string title { get; set; }
        //public string link { get; set; }
        public string description { get; set; }
        //public string language { get; set; }
        //public string copyright { get; set; }
        public string pubDate { get; set; }
        //public string dcdate { get; set; }
        //public string dclanguage { get; set; }
        //public string dcrights { get; set; }
        //public AtomLink atomlink { get; set; }
        //public Image image { get; set; }
        public List<Item> item { get; set; }
    }

    public class Rss
    {
        //public string xmlnsatom { get; set; }
        //public string xmlnsdc { get; set; }
        //public string xmlnsgeorss { get; set; } 
        //public string version { get; set; }
        public Channel channel { get; set; } 
    }

    public class BBCRootObject
    {
        //public Xml xml { get; set; }
        public Rss rss { get; set; }
    }
}
