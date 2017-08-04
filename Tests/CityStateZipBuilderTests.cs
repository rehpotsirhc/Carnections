using GoogleDistance.Models;
using System;
using Xunit;

namespace Tests
{
    public class CityStateZipBuilderTests
    {
        ///All these tests us this format as input:
        ///Salt Lake City, UT, 84111
        [Fact]
        public void FromLocationString1()
        {
            //With state abbreviation: UT or CA
            BasicTest("Salt  Lake City, UT, 84111, USA", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");
            BasicTest("city,ca,84119 USA", "city", "CA", "84119", "city, CA 84119");
            BasicTest("city,ca,USa", "city", "CA", "", "city, CA");
            BasicTest(" city,ca,  ", "city", "CA", "", "city, CA");
            BasicTest(",ca, 84119", "", "CA", "84119", "CA 84119");
            BasicTest("  ,ca,84119", "", "CA", "84119", "CA 84119");
            BasicTest("city,,84119", "city", "", "84119", "city 84119");
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119");
            BasicTest("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTest("Salt  Lake City, UT, 84111,USA", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);
            BasicTest("city,cA,84119 USA", "city", "California", "84119", "city, California 84119", StateNameForm.Full);
            BasicTest("city,ca,            usa", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest(" city,ca,  ", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest(",ca, 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("  ,CA,84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("city,,84119", "city", "", "84119", "city 84119", StateNameForm.Full);
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119", StateNameForm.Full);
            BasicTest("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);



            //With full state name (with a space): New York
            BasicTest("Salt  Lake City, New York, 84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111");
            BasicTest("city,new   York,84119", "city", "New York", "84119", "city, New York 84119");
            BasicTest("city,NEW york         ,", "city", "New York", "", "city, New York");
            BasicTest(" city,new york,  ", "city", "New York", "", "city, New York");
            BasicTest(",New YorK, 84119", "", "New York", "84119", "New York 84119");
            BasicTest("  ,New York,84119", "", "New York", "84119", "New York 84119");
            BasicTest("city,,84119", "city", "", "84119", "city 84119");
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119");
            BasicTest("Salt        Lake City,    New York,    84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111");

            //With full state name (with a space): New York - Like above, but with telling the output to use the abbreviation: NY
            //To accomplish this, the state name form needs to be specified since by default the state form given as input is used as output, e.g., abbreviated form results in the abbreviated form as output
            BasicTest("Salt  Lake City, New York, 84111", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", StateNameForm.Abbreviation);
            BasicTest("city,new   York,84119", "city", "NY", "84119", "city, NY 84119", StateNameForm.Abbreviation);
            BasicTest("city,NEW york         ,", "city", "NY", "", "city, NY", StateNameForm.Abbreviation);
            BasicTest(" city,new york,  ", "city", "NY", "", "city, NY", StateNameForm.Abbreviation);
            BasicTest(",New YorK, 84119", "", "NY", "84119", "NY 84119", StateNameForm.Abbreviation);
            BasicTest("  ,New York,84119", "", "NY", "84119", "NY 84119", StateNameForm.Abbreviation);
            BasicTest("city,,84119", "city", "", "84119", "city 84119", StateNameForm.Abbreviation);
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119", StateNameForm.Abbreviation);
            BasicTest("Salt        Lake City,    NY,    84111   ,usa", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", StateNameForm.Abbreviation);

        }
        [Fact]
        public void FromLocationString1_WildZips()
        {
            //With state abbreviation: UT or CA
            BasicTest("Salt  84119Lake City, UT, 84111, USA", "Salt Lake City", "UT", "84119", "Salt Lake City, UT 84119");
            BasicTest("city,ca,84111 84119 USA", "city", "CA", "84111", "city, CA 84111");
            BasicTest("city,ca,USa 90210", "city", "CA", "90210", "city, CA 90210");
            BasicTest(" city85117,ca,  ", "city", "CA", "85117", "city, CA 85117");
            BasicTest(",ca, 84119", "", "CA", "84119", "CA 84119");
            BasicTest("  ,ca,84119", "", "CA", "84119", "CA 84119");
            BasicTest("city,,84119", "city", "", "84119", "city 84119");
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119");
            BasicTest("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTest("Salt  Lake City, UT, 84111,USA", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);
            BasicTest("city,cA,84119 USA", "city", "California", "84119", "city, California 84119", StateNameForm.Full);
            BasicTest("84112city,ca,            usa", "city", "California", "84112", "city, California 84112", StateNameForm.Full);
            BasicTest(" city,84112 ca,  ", "city", "California", "84112", "city, California 84112", StateNameForm.Full);
            BasicTest(",ca, 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("  ,CA,84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("city,,84119", "city", "", "84119", "city 84119", StateNameForm.Full);
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119", StateNameForm.Full);
            BasicTest("Salt    90210    Lake City,    UT,    84111   ", "Salt Lake City", "Utah", "90210", "Salt Lake City, Utah 90210", StateNameForm.Full);



            //With full state name (with a space): New York
            BasicTest("Salt  Lake City, New York, 84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111");
            BasicTest("city,new   York,84119", "city", "New York", "84119", "city, New York 84119");
            BasicTest("city,NEW york         11111,", "city", "New York", "11111", "city, New York 11111");
            BasicTest(" city,new york33333,  44444", "city", "New York", "33333", "city, New York 33333");
            BasicTest(",New YorK, 84119", "", "New York", "84119", "New York 84119");
            BasicTest("  ,New York,84119", "", "New York", "84119", "New York 84119");
            BasicTest("city,,84119", "city", "", "84119", "city 84119");
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119");
            BasicTest("Salt        Lake City,    New York,    84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111");

            //With full state name (with a space): New York - Like above, but with telling the output to use the abbreviation: NY
            //To accomplish this, the state name form needs to be specified since by default the state form given as input is used as output, e.g., abbreviated form results in the abbreviated form as output
            BasicTest("Salt  Lake City, New York, 84111", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", StateNameForm.Abbreviation);
            BasicTest("city,new   York,84119", "city", "NY", "84119", "city, NY 84119", StateNameForm.Abbreviation);
            BasicTest("city 90210,NEW york         ,", "city", "NY", "90210", "city, NY 90210", StateNameForm.Abbreviation);
            BasicTest(" 77777city,new york,  ", "city", "NY", "77777", "city, NY 77777", StateNameForm.Abbreviation);
            BasicTest(",New YorK, 84119", "", "NY", "84119", "NY 84119", StateNameForm.Abbreviation);
            BasicTest("  ,New York,84119", "", "NY", "84119", "NY 84119", StateNameForm.Abbreviation);
            BasicTest("city,77777,84119", "city", "", "77777", "city 77777", StateNameForm.Abbreviation);
            BasicTest("city,  ,84119", "city", "", "84119", "city 84119", StateNameForm.Abbreviation);
            BasicTest("Salt        Lake City,    NY,    84111   ,usa", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", StateNameForm.Abbreviation);

        }

        [Fact]
        public void FromLocationString2()
        {
            BasicTest("Salt Lake City, UT 84111 usa", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");
            BasicTest(" city,ca 84119", "city", "CA", "84119", "city, CA 84119");
            BasicTest("city,ca", "city", "CA", "", "city, CA");
            BasicTest("city ,ca   ", "city", "CA", "", "city, CA");
            BasicTest(",ca 84119", "", "CA", "84119", "CA 84119");
            BasicTest("  ,ca 84119", "", "CA", "84119", "CA 84119");
            BasicTest("city, cA  ", "city", "CA", "", "city, CA");
            BasicTest("Salt        Lake City,    UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");

            BasicTest("Salt Lake City, UT 84111 , USA         ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);
            BasicTest(" city,ca 84119", "city", "California", "84119", "city, California 84119", StateNameForm.Full);
            BasicTest("city,ca", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest("city ,ca   ", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest(",ca 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("  ,ca 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("city, cA  ", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest("Salt        Lake City,    UT     84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);

            BasicTest("Salt Lake City, Utah 84111 ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", StateNameForm.Abbreviation);
            BasicTest(" city,california 84119", "city", "CA", "84119", "city, CA 84119", StateNameForm.Abbreviation);
            BasicTest("city,California USA", "city", "CA", "", "city, CA", StateNameForm.Abbreviation);
            BasicTest("city ,California   ", "city", "CA", "", "city, CA", StateNameForm.Abbreviation);
            BasicTest(",CALIFORNIA 84119,usA", "", "CA", "84119", "CA 84119", StateNameForm.Abbreviation);
            BasicTest("  ,califorNia 84119", "", "CA", "84119", "CA 84119", StateNameForm.Abbreviation);
            BasicTest("city, california  ", "city", "CA", "", "city, CA", StateNameForm.Abbreviation);
            BasicTest("Salt        Lake City,    UTah     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", StateNameForm.Abbreviation);
        }

        //Salt Lake City, UT 84111
        [Fact]
        public void FromLocationString2_WildZips()
        {
            BasicTest("Salt Lake City, UT 84111 usa", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");
            BasicTest(" city,ca 84119", "city", "CA", "84119", "city, CA 84119");
            BasicTest("city,ca", "city", "CA", "", "city, CA");
            BasicTest("city ,ca   ", "city", "CA", "", "city, CA");
            BasicTest(",ca 84119", "", "CA", "84119", "CA 84119");
            BasicTest("  ,ca 90210 84119", "", "CA", "90210", "CA 90210");
            BasicTest(" 841118city, cA  ", "8city", "CA", "84111", "8city, CA 84111");
            BasicTest("Salt        Lake City,    UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111");

            BasicTest("Salt Lake City, UT 84111 , USA         ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);
            BasicTest(" city,ca 84119", "city", "California", "84119", "city, California 84119", StateNameForm.Full);
            BasicTest("city,ca", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest("city ,ca   ", "city", "California", "", "city, California", StateNameForm.Full);
            BasicTest(",ca 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("  ,ca 84119", "", "California", "84119", "California 84119", StateNameForm.Full);
            BasicTest("city, c84111A  ", "city", "California", "84111", "city, California 84111", StateNameForm.Full);
            BasicTest("Salt        Lake City,    UT     84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", StateNameForm.Full);

            BasicTest("Salt Lake City, Utah 84111 ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", StateNameForm.Abbreviation);
            BasicTest(" city,california 84119", "city", "CA", "84119", "city, CA 84119", StateNameForm.Abbreviation);
            BasicTest("city,California USA", "city", "CA", "", "city, CA", StateNameForm.Abbreviation);
            BasicTest("city ,California   ", "city", "CA", "", "city, CA", StateNameForm.Abbreviation);
            BasicTest("841124,CALIFORNIA 84119,usA", "4", "CA", "84112", "4, CA 84112", StateNameForm.Abbreviation);
            BasicTest("  ,califorNia 84119", "", "CA", "84119", "CA 84119", StateNameForm.Abbreviation);
            BasicTest("city, 22222 california  841118", "city", "CA", "22222", "city, CA 22222", StateNameForm.Abbreviation);
            BasicTest("Salt        Lake City,    UTah     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", StateNameForm.Abbreviation);
        }


        //Salt Lake City, UT
        [Fact]
        public void FromLocationString3()
        {
            BasicTest("Denver, Colorado, USA", "Denver", "Colorado", "", "Denver, Colorado");
            BasicTest(" Denver , Colorado usa", "Denver", "Colorado", "", "Denver, Colorado");
            BasicTest(" Denver  ,           Colorado ", "Denver", "Colorado", "", "Denver, Colorado");
            BasicTest("S L C, Colorado", "S L C", "Colorado", "", "S L C, Colorado");
        }

        //Salt Lake City, UT
        [Fact]
        public void FromLocationString3_WildZips()
        {
            BasicTest("Denver, Colorado, USA 90210", "Denver", "Colorado", "90210", "Denver, Colorado 90210");
            BasicTest(" Denver , 33333Colorado usa", "Denver", "Colorado", "33333", "Denver, Colorado 33333");
            BasicTest(" Denver  33333,           Colorado ", "Denver", "Colorado", "33333", "Denver, Colorado 33333");
            BasicTest("S33333 L C, Colorado", "S L C", "Colorado", "33333", "S L C, Colorado 33333");
        }

        //Salt Lake City 84111
        [Fact]
        public void FromLocationString4()
        {
            BasicTest("Denver 88888, USA", "Denver", "", "88888", "Denver 88888");
            BasicTest(" Denver 77777 usa", "Denver", "", "77777", "Denver 77777");
            BasicTest(" DEnver     77777", "DEnver", "", "77777", "DEnver 77777");
            BasicTest("S L C 11111", "S L C", "", "11111", "S L C 11111");

        }

        //Salt Lake City 84111
        [Fact]
        public void FromLocationString4_WildZips()
        {
            BasicTest("Denver 88888, USA 66666", "Denver", "", "88888", "Denver 88888");
            BasicTest(" Denv66666er 77777 usa", "Denver", "", "66666", "Denver 66666");
            BasicTest(" 66666DEnver     77777", "DEnver", "", "66666", "DEnver 66666");
            BasicTest("S L C 66666 11111", "S L C", "", "66666", "S L C 66666");

        }

        //84111
        [Fact]
        public void FromLocationString5()
        {
            BasicTest("11111", "", "", "11111", "11111");
            BasicTest(" 11111 ", "", "", "11111", "11111");
            BasicTest("11111         ", "", "", "11111", "11111");
            BasicTest("        11111", "", "", "11111", "11111");
        }

        [Fact]
        public void FromLocationString5_WildZips()
        {
            BasicTest("11111 22222", "", "", "11111", "11111");
            BasicTest("22222 11111 ", "", "", "22222", "22222");
            BasicTest("511111  33   11    ", "1 33 11", "", "51111", "1 33 11 51111");
            BasicTest("        111118", "8", "", "11111", "8 11111");
        }



        private void BasicTest(string location, string expectedCity, string expectedState, string expectedZip, string expectedLocation, StateNameForm? stateNameFormOverride = null)
        {
            var cityStateZip = CityStateZipBuilder.Build(location, stateNameFormOverride);
            Assert.Equal(expectedCity, cityStateZip.City);
            Assert.Equal(expectedState, cityStateZip.State);
            Assert.Equal(expectedZip, cityStateZip.Zip);
            Assert.Equal(expectedLocation, cityStateZip.FullAddress);
        }


    }
}


