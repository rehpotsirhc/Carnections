using System;
using Common.Interfaces;
using Common.Models;
using System.Text.RegularExpressions;
using System.Linq;


namespace GoogleDistance.Models
{
    public static class CityStateZipExtensions
    {
        public static string RemoveWhiteSpace(this string myString)
        {
            return Regex.Replace(myString, @"\s+", " ");
        }
        public static bool MatchZip(this string zip)
        {
            return !String.IsNullOrWhiteSpace(zip) && Regex.Match(@"^\d{5}$", zip).Success;
        }

        public static string FindAndRemoveZip(this string inputString, out string zip)
        {
            string pattern = @"\d{5}";
            zip = Regex.Match(inputString, pattern).Value;
            return Regex.Replace(inputString, pattern, "");
        }
        public static string RemoveUSA(this string fullLocationString)
        {
            if (fullLocationString.EndsWith(", USA", StringComparison.CurrentCultureIgnoreCase))
                return fullLocationString.Substring(0, fullLocationString.Length - 5);
            else if (fullLocationString.EndsWith(" USA", StringComparison.CurrentCultureIgnoreCase) || fullLocationString.EndsWith(",USA", StringComparison.CurrentCultureIgnoreCase))
                return fullLocationString.Substring(0, fullLocationString.Length - 4);
            else return fullLocationString;
        }
    }


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
        private CityStateZipBuilder(string fullLocationString, StateNameForm stateNameFormOverride = StateNameForm.NoPreference)
        {
            string city = "", state = "", zip = "";
            if (String.IsNullOrWhiteSpace(fullLocationString = fullLocationString.RemoveWhiteSpace().Trim()))
                return;

  
            //since we're removing all zipcodes upfront, we probably don't need to look for them later on in the method, but...
            //we there are expensive unit tests covering this class that demonstrate it works, so leave it for now
            fullLocationString = fullLocationString.FindAndRemoveZip(out string masterZip).RemoveWhiteSpace().Trim().RemoveUSA();

            if (!String.IsNullOrWhiteSpace(fullLocationString))
            {
                string[] parts = fullLocationString.Split(',').Select(s => s.Trim()).ToArray();

                //Salt Lake City 84111
                //84111
                if (parts.Length == 1)
                {
                    city = parts[0];
                    //string[] cityZip = parts[0].Split(' ');

                    //if (cityZip.Length >= 1)
                    //{
                    //    city = cityZip[0];
                    //}
                    //if (cityZip.Length > 1)
                    //{
                    //    zip = cityZip[1];
                    //}
                }
                //Salt Lake City, UT 84111
                //Salt Lake City, UT
                else if (parts.Length == 2)
                {
                    city = parts[0];

                    if (StateBuilder.IsState(parts[1]))
                        state = parts[1];
                    else
                    {
                        string[] stateZip = parts[1].Split(' ');

                        if (stateZip.Length >= 1)
                            state = stateZip[0];

                        if (stateZip.Length > 1)
                            zip = stateZip[1];
                    }
                }
                else if (parts.Length >= 3)
                {
                    city = parts[0];
                    state = parts[1];
                    zip = parts[2];
                }
            }

            this.City = city;
            this.State = state;
            this.StateAsString = this.State == null ? "" : State.ToString(stateNameFormOverride);
            this.Zip = String.IsNullOrWhiteSpace(masterZip) ? zip : zip.MatchZip() ? zip : masterZip;
        }



        private CityStateZipBuilder(string city = null, string state = null, string zip = null, StateNameForm stateNameFormOverride = StateNameForm.NoPreference)
        {
            this.City = String.IsNullOrWhiteSpace(city) ? "" : city.RemoveWhiteSpace().Trim();
            this.State = String.IsNullOrWhiteSpace(state) ? "" : state.RemoveWhiteSpace().Trim();
            this.StateAsString = this.State == null ? "" : State.ToString(stateNameFormOverride);
            this.Zip = String.IsNullOrWhiteSpace(zip) ? "" : zip.RemoveWhiteSpace().Trim();
        }

        public static ICityStateZipWithString Build(string fullLocationString, StateNameForm stateNameFormOverride = StateNameForm.NoPreference)
        {
            var builder = new CityStateZipBuilder(fullLocationString, stateNameFormOverride);
            return CityStateZipBuilder.Build(builder);
        }

        public static ICityStateZipWithString Build(string city = null, string state = null, string zip = null, StateNameForm stateNameFormOverride = StateNameForm.NoPreference)
        {
            var builder = new CityStateZipBuilder(city, state, zip, stateNameFormOverride);
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


        public static bool Equals(ICityStateZip cityStateZip1, ICityStateZip cityStateZip2)
        {
            return cityStateZip1.City.RemoveWhiteSpace().Trim().Equals(cityStateZip2.City.RemoveWhiteSpace().Trim(), StringComparison.CurrentCultureIgnoreCase) &&
                (new StateBuilder(cityStateZip2.State).Equals(new StateBuilder(cityStateZip2.State)) &&
                cityStateZip1.Zip.RemoveWhiteSpace().Trim().Equals(cityStateZip2.Zip.RemoveWhiteSpace().Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public static int GetHashCode(ICityStateZip cityStateZip)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + cityStateZip.City.GetHashCode();
                hash = hash * 23 + (new StateBuilder(cityStateZip.State).GetHashCode());
                hash = hash * 23 + cityStateZip.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var that = (CityStateZipBuilder)obj;
            return CityStateZipBuilder.Equals(CityStateZipBuilder.Build(this), CityStateZipBuilder.Build(that));
        }

        public override int GetHashCode()
        {
            return CityStateZipBuilder.GetHashCode(CityStateZipBuilder.Build(this));
        }

        ///Salt Lake City, UT, 84111
        //Salt Lake City, UT 84111
        //Salt Lake City, UT
        //Salt Lake City 84111
        //84111
        //Salt Lake City
        //Ut
        public override string ToString()
        {
            bool cityBlank = String.IsNullOrWhiteSpace(City);
            bool stateBlank = String.IsNullOrWhiteSpace(StateAsString);
            bool zipBlank = String.IsNullOrWhiteSpace(Zip);

            string city = cityBlank ? "" : City + ", ";
            string state = stateBlank ? "" : StateAsString + " ";
            string zip = zipBlank ? "" : Zip;


            //Salt Lake City 84111
            if (stateBlank && !cityBlank)
                city = city.Replace(",", "");

            return (city + state + zip).Trim();
        }



    }
}


