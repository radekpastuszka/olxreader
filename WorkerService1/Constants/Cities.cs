using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService1.Constants
{
    public class Cities
    {
        public const string Total = "";
        public const string Wroclaw = "wroclaw";
        public const string Gdansk = "gdansk";
        public const string Warszawa = "warszawa";
        public const string Krakow = "krakow";
        public const string Poznan = "poznan";
        public const string Lodz = "lodz";

        public static List<string> GetCities()
        {
            List<string> cities = new List<string>();
            cities.Add(Total);
            cities.Add(Wroclaw);
            cities.Add(Gdansk);
            cities.Add(Warszawa);
            cities.Add(Krakow);
            cities.Add(Poznan);
            cities.Add(Lodz);

            return cities;
        }
    }
}
