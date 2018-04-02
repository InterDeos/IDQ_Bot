using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Structs.PlanetRomeo
{
    struct Area
    {
        private string name;
        private string id;

        public string Name { get => name; set => name = value; }
        public string Id { get => id; set => id = value; }

        public Area(string _name, string _id)
        {
            name = _name;
            id = _id;
        }
    }
    struct Country
    {
        private string name;
        private string id;
        private List<Area> areaes;

        public string Name { get => name; set => name = value; }
        public string Id { get => id; set => id = value; }
        public List<Area> Areaes { get => areaes; set => areaes = value; }

        public Country(string _name, string _id, List<Area> _areaes)
        {
            name = _name;
            id = _id;
            areaes = _areaes;
        }
    }
    struct Continent
    {
        private string name;
        private string id;
        private List<Country> countryes;

        public string Name { get => name; set => name = value; }
        public string Id { get => id; set => id = value; }
        public List<Country> Countryes { get => countryes; set => countryes = value; }

        public Continent(string _name, string _id, List<Country> _countryes)
        {
            name = _name;
            id = _id;
            countryes = _countryes;
        }
    }
}
