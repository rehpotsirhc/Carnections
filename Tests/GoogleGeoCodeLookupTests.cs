using GoogleDistance;
using Xunit;
using Common.Models;
using Common.Interfaces;
using Common.Enums;

namespace Tests
{
    public class GoogleGeoCodeLookupTests
    {
        [Fact]
        public async void SimpleSuccessTest_1()
        {
            ILocation expected = new Location()
            {
                City = "salt Lake city",
                State = "Ut",
                Zip = "84119",
                Latitude = 40.71,
                Longitude = -111.96
            };

            var actual = await LookupLonLat.Lookup(expected.City, expected.State, expected.Zip, EStateNameForm.Abbreviation);

            LocationBasicTest(actual, expected);
        }

        [Fact]
        public async void SimpleSuccessTest_2()
        {
            ILocation expected = new Location()
            {
                City = "salt Lake city",
                State = "Ut",
                Zip = "84119",
                Latitude = 40.71,
                Longitude = -111.96
            };

            var actual = await LookupLonLat.Lookup(new CityStateZip()
            {
                City = expected.City,
                State = expected.State,
                Zip = expected.Zip
            },
                EStateNameForm.Abbreviation);

            LocationBasicTest(actual, expected);
        }


        [Fact]
        public async void SimpleSuccessTest_3()
        {
            ILocation expected = new Location()
            {
                City = "salt Lake city",
                State = "Ut",
                Zip = "84119",
                Latitude = 40.71,
                Longitude = -111.96
            };
            var actual = await LookupLonLat.Lookup(expected.City + " " + expected.State + " " + expected.Zip, EStateNameForm.Abbreviation);
            LocationBasicTest(actual, expected);

        }

        [Fact]
        public async void SimpleSuccessTest_4()
        {
            ILocation expected = new Location()
            {
                City = "salt Lake city",
                State = "Ut",
                Zip = "84119",
                Latitude = 40.71,
                Longitude = -111.96
            };
            var actual = await LookupLonLat.Lookup(expected.City + ", " + expected.State + " " + expected.Zip, EStateNameForm.Abbreviation);
            LocationBasicTest(actual, expected);
        }

        [Fact]
        public async void SimpleSuccessTest_5()
        {
            ILocation expected = new Location()
            {
                City = "salt Lake city",
                State = "Ut",
                Latitude = 40.76,
                Longitude = -111.89
            };
            var actual = await LookupLonLat.Lookup(expected.City + ", " + expected.State, EStateNameForm.Abbreviation);
            CityBasicTest(actual, expected);
            StateBasicTest(actual, expected);
            LatLonBasicTest(actual, expected);
        }

        [Fact]
        public async void SimpleSuccessTest_6()
        {
            ILocation expected = new Location()
            {
                Zip = "84119",
                Latitude = 40.71,
                Longitude = -111.96
            };
            var actual = await LookupLonLat.Lookup(expected.Zip, EStateNameForm.Abbreviation);
            ZipBasicTest(actual, expected);
            LatLonBasicTest(actual, expected);
        }


        [Fact]
        public async void ComplexSuccessTest_1()
        {
            ILocation expected = new Location()
            {
                City = "St Louis",
                State = "MO",
                Latitude = 38.63,
                Longitude = -90.20
            };

            var actual = await LookupLonLat.Lookup(expected.City, expected.State, expected.Zip, EStateNameForm.Abbreviation);

            CityBasicTest(actual, expected);
            StateBasicTest(actual, expected);
            LatLonBasicTest(actual, expected);
        }

        [Fact]
        public async void ComplexSuccessTest_2()
        {
            ILocation expected = new Location()
            {
                City = "Saint Louis",
                State = "MO",
                Latitude = 38.63,
                Longitude = -90.20
            };

            var actual = await LookupLonLat.Lookup(expected.City, expected.State, expected.Zip, EStateNameForm.Abbreviation);


            StateBasicTest(actual, expected);
            LatLonBasicTest(actual, expected);
        }

        private void LocationBasicTest(ILocation actual, ILocation expected)
        {
            Assert.NotNull(actual);
            CityBasicTest(actual, expected);
            StateBasicTest(actual, expected);
            ZipBasicTest(actual, expected);
            LatLonBasicTest(actual, expected);
        }

        private void LatLonBasicTest(ILocation actual, ILocation expected)
        {
            Assert.Equal(expected.Latitude, actual.Latitude, 2);
            Assert.Equal(expected.Longitude, actual.Longitude, 2);
        }

        private void CityBasicTest(ILocation actual, ILocation expected)
        {
            Assert.Equal(expected.City.ToLower(), actual.City.ToLower());
        }

        private void StateBasicTest(ILocation actual, ILocation expected)
        {
            Assert.Equal(expected.State.ToLower(), actual.State.ToLower());
        }

        private void ZipBasicTest(ILocation actual, ILocation expected)
        {
            Assert.Equal(expected.Zip.ToLower(), actual.Zip.ToLower());
        }


    }
}
