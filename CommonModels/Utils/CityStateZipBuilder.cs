using System;
using Common.Interfaces;
using Common.Models;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Common.Enums;

namespace GoogleDistance.Models
{
    public static class CityStateZipExtensions
    {
        public static bool EqualsClean(this string inputString1, string inputString2)
        {
            return inputString1.RemoveWhiteSpace().Trim().Equals(inputString2.RemoveWhiteSpace().Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool CaseInsensitiveContains(this string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
        public static string RemoveWhiteSpace(this string inputString)
        {
            return Regex.Replace(inputString, @"\s+", " ");
        }
        public static bool MatchZip(this string zip)
        {
            return !String.IsNullOrWhiteSpace(zip) && Regex.Match(@"^\d{5}$", zip).Success;
        }
        public static string RemoveAllButFirst(this string inputString, char c)
        {
            int pos = 1 + inputString.IndexOf(c);
            return inputString.Substring(0, pos) + inputString.Substring(pos).Replace(",", string.Empty);
        }

        public static string FindAndRemoveZip(this string inputString, out string zip)
        {
            string pattern = @"\d{5}";
            zip = Regex.Match(inputString, pattern).Value;
            return Regex.Replace(inputString, pattern, "");
        }
        public static string RemovefromEnd(this string inputString, char c)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return inputString;
            string temp = inputString.TrimEnd();
            int iLastChar = temp.Length - 1;
            if (iLastChar >= 0 && temp[iLastChar] == c)
            {
                return inputString.Remove(iLastChar, 1);
            }
            return inputString;

        }
        public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        public static string RemoveUSA(this string fullLocationString)
        {
            if (fullLocationString.CaseInsensitiveContains(", USA", StringComparison.CurrentCultureIgnoreCase))
                return fullLocationString.Substring(0, fullLocationString.Length - 5);
            else if (fullLocationString.CaseInsensitiveContains(" USA", StringComparison.CurrentCultureIgnoreCase) || fullLocationString.CaseInsensitiveContains(",USA", StringComparison.CurrentCultureIgnoreCase))
                return fullLocationString.Substring(0, fullLocationString.Length - 4);
            else return fullLocationString;
        }
        public static string RemoveSpecialCharacters(this string inputString)
        {
            string pattern = @"[^a-zA-Z\s,]";
            return Regex.Replace(inputString, pattern, "");
        }
    }


    public class CityStateZipBuilder :IEquatable<CityStateZipBuilder>
    {
        public static readonly int ZIP_DEGREE = 3;
        public static readonly int CITY_DEGREE = 2;
        public static readonly int STATE_DEGREE = 1;


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
        private CityStateZipBuilder(string fullLocationString, EStateNameForm stateNameFormOverride = EStateNameForm.NoPreference)
        {
            string city = "", state = "", zip = "";
            if (String.IsNullOrWhiteSpace(fullLocationString = fullLocationString.RemoveWhiteSpace().Trim()))
                return;


            //since we're removing all zipcodes upfront, we probably don't need to look for them later on in the method, but...
            //we there are expensive unit tests covering this class that demonstrate it works, so leave it for now
            fullLocationString = fullLocationString.FindAndRemoveZip(out string masterZip).RemoveUSA().RemoveAllButFirst(',').RemoveSpecialCharacters().RemovefromEnd(',').RemoveWhiteSpace().Trim();


            if (!String.IsNullOrWhiteSpace(fullLocationString))
            {
                //will contain 0 or 1 commas, not more
                string[] parts = fullLocationString.Split(',').Select(s => s.Trim()).ToArray();


                if (parts.Length == 1)
                {
                    bool found = false;
                    foreach (int i in parts[0].AllIndexesOf(" "))
                    {

                        if (i != -1)
                        {
                            string potentialCity = parts[0].Substring(0, i);
                            string potentialState = parts[0].Substring(i + 1);

                            if (StateBuilder.IsState(potentialState))
                            {
                                state = potentialState;
                                city = potentialCity;
                                found = true;
                                break;
                            }
                            else if (StateBuilder.IsState(potentialCity))
                            {
                                state = potentialCity;
                                city = potentialState;
                                found = true;
                                break;
                            }
                        }

                    }
                    if (!found)
                    {
                        if (StateBuilder.IsState(parts[0]))
                            state = parts[0];
                        else
                            city = parts[0];
                    }
                }

                else
                {
                    city = parts[0];
                    state = parts[1];
                }
            }

            this.City = city;
            this.State = state;
            this.StateAsString = this.State == null ? "" : State.ToString(stateNameFormOverride);
            this.Zip = String.IsNullOrWhiteSpace(masterZip) ? zip : zip.MatchZip() ? zip : masterZip;
        }



        private CityStateZipBuilder(string city = null, string state = null, string zip = null, EStateNameForm stateNameFormOverride = EStateNameForm.NoPreference)
        {
            this.City = String.IsNullOrWhiteSpace(city) ? "" : city.RemoveWhiteSpace().Trim();
            this.State = String.IsNullOrWhiteSpace(state) ? "" : state.RemoveWhiteSpace().Trim();
            this.StateAsString = this.State == null ? "" : State.ToString(stateNameFormOverride);
            this.Zip = String.IsNullOrWhiteSpace(zip) ? "" : zip.RemoveWhiteSpace().Trim();
        }

        public static ICityStateZipWithString Build(string fullLocationString, EStateNameForm stateNameFormOverride = EStateNameForm.NoPreference)
        {
            var builder = new CityStateZipBuilder(fullLocationString, stateNameFormOverride);
            return CityStateZipBuilder.Build(builder);
        }

        public static ICityStateZipWithString Build(string city = null, string state = null, string zip = null, EStateNameForm stateNameFormOverride = EStateNameForm.NoPreference)
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
            return CityEqualsDegree(cityStateZip1, cityStateZip2) > 0 && StateEqualsDegree(cityStateZip1, cityStateZip2) > 0 && ZipEqualsDegree(cityStateZip1, cityStateZip2) > 0;
        }
        private static int CityEqualsDegree(ICityStateZip cityStateZip1, ICityStateZip cityStateZip2)
        {
            if (!String.IsNullOrWhiteSpace(cityStateZip1.City) && cityStateZip1.City.EqualsClean(cityStateZip2.City))
                return CITY_DEGREE;
            else return 0;
        }
        private static int StateEqualsDegree(ICityStateZip cityStateZip1, ICityStateZip cityStateZip2)
        {
            if (!String.IsNullOrWhiteSpace(cityStateZip1.State) && (new StateBuilder(cityStateZip2.State).Equals(new StateBuilder(cityStateZip2.State))))
                return STATE_DEGREE;
            else return 0;

        }
        private static int ZipEqualsDegree(ICityStateZip cityStateZip1, ICityStateZip cityStateZip2)
        {
            if (!String.IsNullOrWhiteSpace(cityStateZip1.Zip) && cityStateZip1.Zip.EqualsClean(cityStateZip2.Zip))
                return ZIP_DEGREE;
            else return 0;
        }


        public static int DegreeOfEquals(ICityStateZip cityStateZip1, ICityStateZip cityStateZip2)
        {
            return CityEqualsDegree(cityStateZip1, cityStateZip2) + StateEqualsDegree(cityStateZip1, cityStateZip2) + ZipEqualsDegree(cityStateZip1, cityStateZip2);
        }

        public static int GetHashCode(ICityStateZip cityStateZip)
        {
            unchecked //to avoid overflowing int
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
            return Equals((CityStateZipBuilder)obj);
        }
        public bool Equals(CityStateZipBuilder that)
        {
            return CityStateZipBuilder.Equals(CityStateZipBuilder.Build(this), CityStateZipBuilder.Build(that));
        }

        public int DegreeOfEquals(CityStateZipBuilder that)
        {
            return CityStateZipBuilder.DegreeOfEquals(CityStateZipBuilder.Build(this), CityStateZipBuilder.Build(that));
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


