using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDQ_Bot.Model.Structs.PlanetRomeo
{
    struct PRSearchSettings
    {
        private string _continent;
        private string _country;
        private string _area;
        private string _minAge;
        private string _maxAge;

        public string Continent { get => _continent; set => _continent = value; }
        public string Country { get => _country; set => _country = value; }
        public string Area { get => _area; set => _area = value; }
        public string MinAge { get => _minAge; set => _minAge = value; }
        public string MaxAge { get => _maxAge; set => _maxAge = value; }

        public PRSearchSettings(string continent, string country, string area, string minAge, string maxAge) : this()
        {
            Continent = continent;
            Country = country;
            Area = area;
            MinAge = minAge;
            MaxAge = maxAge;
        }

        public static bool operator ==(PRSearchSettings op1, PRSearchSettings op2)
        {
            return op1.Equals(op2);
        }

        public static bool operator !=(PRSearchSettings op1, PRSearchSettings op2)
        {
            return !op1.Equals(op2);
        }
    }
}
