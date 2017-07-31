using System;
using Common.Interfaces;
using Common.Models;

namespace GoogleDistance.Models
{

    public class CityStateZipBuilder
    {
        public string City { get; }
        public StateBuilder State { get; }
        public string Zip { get; }
        private string StateAsString { get; }

        /// <summary>
        /// Example strings supported:
        ///Salt Lake City, UT, 84111
        //Salt Lake City, UT 84111
        //Salt Lake City, UT
        //Salt Lake City 84111
        //84111
        /// </summary>
        /// <param name="fullLocationString"></param>
        public CityStateZipBuilder(string fullLocationString)
        {

            if (String.IsNullOrWhiteSpace(fullLocationString))
                return;

            if (fullLocationString.EndsWith(", USA"))
                fullLocationString = fullLocationString.Substring(0, fullLocationString.Length - 5);
            else if (fullLocationString.EndsWith(" USA"))
                fullLocationString = fullLocationString.Substring(0, fullLocationString.Length - 4);



            string[] parts = fullLocationString.Split(',');
            string city = null, state = null, zip = null;

            //Salt Lake City 84111
            //84111
            if (parts.Length == 1)
            {
                string[] cityZip = parts[0].Trim().Split(' ');
                if (cityZip.Length >= 1)
                {
                    zip = cityZip[0].Trim();
                }
                if (cityZip.Length > 1)
                {
                    city = cityZip[0].Trim();
                }
            }
            //Salt Lake City, UT 84111
            //Salt Lake City, UT
            else if (parts.Length == 2)
            {
                city = parts[0].Trim();
                string[] stateZip = parts[1].Trim().Split(' ');

                if (stateZip.Length >= 1)
                {
                    state = stateZip[0].Trim();
                }
                if (stateZip.Length > 1)
                {
                    zip = stateZip[1].Trim();
                }
            }
            else if (parts.Length >= 3)
            {
                city = parts[0].Trim();
                state = parts[1].Trim();
                zip = parts[2].Trim();
            }

            this.City = city;
            this.State = state;
            this.StateAsString = this.State.ToString();
            this.Zip = zip;
        }

        public CityStateZipBuilder(string city = null, string state = null, string zip = null)
        {
            this.City = city;
            this.State = state;
            this.StateAsString = this.State.ToString();
            this.Zip = zip;
        }

        public static ICityStateZipWithString Build(string fullLocationString)
        {
            var builder = new CityStateZipBuilder(fullLocationString);
            return CityStateZipBuilder.Build(builder);
        }

        public static ICityStateZipWithString Build(string city = null, string state = null, string zip = null)
        {
            var builder = new CityStateZipBuilder(city, state, zip);
            return CityStateZipBuilder.Build(builder);
        }

        private static ICityStateZipWithString Build(CityStateZipBuilder builder)
        {
            return new CityStateZipWithString()
            {
                City = builder.City,
                State = builder.StateAsString,
                Zip = builder.Zip,
                FullAddress = builder.ToString()
            };
        }

        ///Salt Lake City, UT, 84111
        //Salt Lake City, UT 84111
        //Salt Lake City, UT
        //Salt Lake City 84111
        //84111
        //Salt Lake City
        //Ut
        public string ToString(StateNameForm stateNameFormOverride)
        {
            bool cityBlank = String.IsNullOrWhiteSpace(City);
            bool stateBlank = String.IsNullOrWhiteSpace(StateAsString);
            bool zipBlank = String.IsNullOrWhiteSpace(Zip);

            string city = cityBlank ? "" : City + ", ";
            string state = stateBlank ? "" : StateAsString + " ";
            string zip = zipBlank ? "" : Zip;


            //Salt Lake City 84111
            if (stateBlank && !zipBlank && !cityBlank)
                city = city.Remove(city.Length - 2);

            return (city + state + zip).Trim();
        }
        public override string ToString()
        {
            return ToString(StateNameForm.NoPreference);
        }
    }
}


