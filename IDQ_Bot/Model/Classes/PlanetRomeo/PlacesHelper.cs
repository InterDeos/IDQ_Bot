using IDQ_Bot.Model.Structs.PlanetRomeo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Classes.PlanetRomeo
{
    static class PlacesHelper
    {
        public static List<string> GetListContinents(this List<Continent> continents)
        {
            var temp = new List<string>();
            foreach (var item in continents)
            {
                temp.Add(item.Name);
            }
            return temp;
        }
        public static List<string> GetListCountries(this List<Continent> continents, string continent)
        {
            var temp = new List<string>();
            foreach (var item in continents.Find((x) => x.Name == continent).Countryes)
            {
                temp.Add(item.Name);
            }
            return temp;
        }
        public static List<string> GetListArea(this List<Continent> continents, string continent, string country)
        {
            var temp = new List<string>();
            if (country != null)
            {
                var tempcountries = continents.Find((x) => x.Name == continent).Countryes;
                foreach (var item in tempcountries.Find((y) => y.Name == country).Areaes)
                {
                    temp.Add(item.Name);
                }
            }
            return temp;
        }
        public static PRSearchSettings ToID(this PRSearchSettings search, List<Continent> continents)
        {
            var contid = continents.Find((x) => x.Name == search.Continent).Id;
            var counid = continents.Find((x) => x.Name == search.Continent).Countryes.Find((y) => y.Name == search.Country).Id;
            var areaid = continents.Find((x) => x.Name == search.Continent).Countryes.Find((y) => y.Name == search.Country).Areaes.Find((z) => z.Name == search.Area).Id;
            return new PRSearchSettings(contid, counid, areaid, search.MinAge, search.MaxAge);
        }
    }
}
