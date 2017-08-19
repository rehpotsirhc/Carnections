using Common.Enums;
using Common.Interfaces;
using GoogleDistance.Models;
using System;
using Xunit;

namespace CityState.Tests
{
    public class CityStateZipBuilderTests
    {
        ///All these tests us this format as input:
        ///Salt Lake City, UT, 84111
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString1()
        {
            //With state abbreviation: UT or CA
            BasicTestFromLocation("Salt  Lake City, UT, 84111, USA", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca,84119 USA", "city", "CA", "84119", "city, CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca,USa", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city,ca,  ", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(",ca, 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,ca,84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTestFromLocation("Salt  Lake City, UT, 84111,USA", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);
            BasicTestFromLocation("city,cA,84119 USA", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,ca,            usa", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(" city,ca,  ", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(",ca, 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("  ,CA,84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);



            //With full state name (with a space): New York
            BasicTestFromLocation("Salt  Lake City, New York, 84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,new   York,84119", "city", "New York", "84119", "city, New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,NEW york         ,", "city", "New York", "", "city, New York", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city,new york,  ", "city", "New York", "", "city, New York", EStateNameForm.NoPreference);
            BasicTestFromLocation(",New YorK, 84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,New York,84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    New York,    84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);

            //With full state name (with a space): New York - Like above, but with telling the output to use the abbreviation: NY
            //To accomplish this, the state name form needs to be specified since by default the state form given as input is used as output, e.g., abbreviated form results in the abbreviated form as output
            BasicTestFromLocation("Salt  Lake City, New York, 84111", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,new   York,84119", "city", "NY", "84119", "city, NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,NEW york         ,", "city", "NY", "", "city, NY", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" city,new york,  ", "city", "NY", "", "city, NY", EStateNameForm.Abbreviation);
            BasicTestFromLocation(",New YorK, 84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("  ,New York,84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("Salt        Lake City,    NY,    84111   ,usa", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);

        }
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString1_WildZips()
        {
            //With state abbreviation: UT or CA
            BasicTestFromLocation("Salt  84119Lake City, UT, 84111, USA", "Salt Lake City", "UT", "84119", "Salt Lake City, UT 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca,84111 84119 USA", "city", "CA", "84111", "city, CA 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca,USa 90210", "city", "CA", "90210", "city, CA 90210", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city85117,ca,  ", "city", "CA", "85117", "city, CA 85117", EStateNameForm.NoPreference);
            BasicTestFromLocation(",ca, 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,ca,84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    UT,    84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTestFromLocation("Salt  Lake City, UT, 84111,USA", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);
            BasicTestFromLocation("city,cA,84119 USA", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("84112city,ca,            usa", "city", "California", "84112", "city, California 84112", EStateNameForm.Full);
            BasicTestFromLocation(" city,84112 ca,  ", "city", "California", "84112", "city, California 84112", EStateNameForm.Full);
            BasicTestFromLocation(",ca, 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("  ,CA,84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("Salt    90210    Lake City,    UT,    84111   ", "Salt Lake City", "Utah", "90210", "Salt Lake City, Utah 90210", EStateNameForm.Full);



            //With full state name (with a space): New York
            BasicTestFromLocation("Salt  Lake City, New York, 84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,new   York,84119", "city", "New York", "84119", "city, New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,NEW york         11111,", "city", "New York", "11111", "city, New York 11111", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city,new york33333,  44444", "city", "New York", "33333", "city, New York 33333", EStateNameForm.NoPreference);
            BasicTestFromLocation(",New YorK, 84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,New York,84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    New York,    84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);

            //With full state name (with a space): New York - Like above, but with telling the output to use the abbreviation: NY
            //To accomplish this, the state name form needs to be specified since by default the state form given as input is used as output, e.g., abbreviated form results in the abbreviated form as output
            BasicTestFromLocation("Salt  Lake City, New York, 84111", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,new   York,84119", "city", "NY", "84119", "city, NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city 90210,NEW york         ,", "city", "NY", "90210", "city, NY 90210", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" 77777city,new york,  ", "city", "NY", "77777", "city, NY 77777", EStateNameForm.Abbreviation);
            BasicTestFromLocation(",New YorK, 84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("  ,New York,84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,77777,84119", "city", "", "77777", "city 77777", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,  ,84119", "city", "", "84119", "city 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("Salt        Lake City,    NY,    84111   ,usa", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);

        }


        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString1_NoCommas()
        {
            //With state abbreviation: UT or CA
            BasicTestFromLocation("Salt  Lake City UT 84111, USA", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ca 84119 USA", "city", "CA", "84119", "city, CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ca USa", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city ca  ", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(" ca citya cityb 84119", "citya cityb", "CA", "84119", "citya cityb, CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("   ca84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City     UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTestFromLocation("Salt  Lake City  UT  84111 USA", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);
            BasicTestFromLocation("city cA 84119 USA", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city ca,            usa", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(" city ca,  ", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(" ca  84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("   CA 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("Salt        Lake City     UT     84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);



            //With full state name (with a space): New York
            BasicTestFromLocation("Salt  Lake City  New York  84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city new   York 84119", "city", "New York", "84119", "city, New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city NEW york         ,", "city", "New York", "", "city, New York", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city new york   ", "city", "New York", "", "city, New York", EStateNameForm.NoPreference);
            BasicTestFromLocation(" New YorK  84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("   New York 84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City     New York     84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);

            //With full state name (with a space): New York - Like above, but with telling the output to use the abbreviation: NY
            //To accomplish this, the state name form needs to be specified since by default the state form given as input is used as output, e.g., abbreviated form results in the abbreviated form as output
            BasicTestFromLocation("Salt  Lake City  New York  84111", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city new   York 84119", "city", "NY", "84119", "city, NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city NEW york         ,", "city", "NY", "", "city, NY", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" city new york,  ", "city", "NY", "", "city, NY", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" New YorK 84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("   New York 84119", "", "NY", "84119", "NY 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city 84119", "city", "", "84119", "city 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("Salt        Lake City     NY     84111    usa", "Salt Lake City", "NY", "84111", "Salt Lake City, NY 84111", EStateNameForm.Abbreviation);

        }
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString1_WildZips_NoCommas()
        {
            //With state abbreviation: UT or CA
            BasicTestFromLocation("Salt  84119Lake City  UT  84111  USA", "Salt Lake City", "UT", "84119", "Salt Lake City, UT 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ca 84111 84119 USA", "city", "CA", "84111", "city, CA 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ca USa 90210", "city", "CA", "90210", "city, CA 90210", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city85117 ca   ", "city", "CA", "85117", "city, CA 85117", EStateNameForm.NoPreference);
            BasicTestFromLocation(" ca  84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("   ca 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City     UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            //With state abbreviation: UT or CA but with telling the output to use the full state name
            BasicTestFromLocation("city cA 84119 USA", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("84112city ca             usa", "city", "California", "84112", "city, California 84112", EStateNameForm.Full);
            BasicTestFromLocation(" city 84112 ca   ", "city", "California", "84112", "city, California 84112", EStateNameForm.Full);
            BasicTestFromLocation(" ca  84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("   CA 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.Full);
            BasicTestFromLocation("Salt    90210    Lake City     UT     84111   ", "Salt Lake City", "Utah", "90210", "Salt Lake City, Utah 90210", EStateNameForm.Full);



            //With full state name (with a space): New York
            BasicTestFromLocation("Salt  Lake City  New York  84111 USA", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("city new   York 84119", "city", "New York", "84119", "city, New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city NEW york         11111 ", "city", "New York", "11111", "city, New York 11111", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city new york33333   44444", "city", "New York", "33333", "city, New York 33333", EStateNameForm.NoPreference);
            BasicTestFromLocation(" New YorK  84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("   New York 84119", "", "New York", "84119", "New York 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city  84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city    84119", "city", "", "84119", "city 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City     New York     84111   ", "Salt Lake City", "New York", "84111", "Salt Lake City, New York 84111", EStateNameForm.NoPreference);


        }

        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString2()
        {
            BasicTestFromLocation("Salt Lake City, UT 84111 usa", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city,ca 84119", "city", "CA", "84119", "city, CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ,ca   ", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(",ca 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,ca 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city, cA  ", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            BasicTestFromLocation("Salt Lake City, UT 84111 , USA         ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);
            BasicTestFromLocation(" city,ca 84119", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,ca", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation("city ,ca   ", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(",ca 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("  ,ca 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city, cA  ", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation("Salt        Lake City,    UT     84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);

            BasicTestFromLocation("Salt Lake City, Utah 84111 ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" city,california 84119", "city", "CA", "84119", "city, CA 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,California USA", "city", "CA", "", "city, CA", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city ,California   ", "city", "CA", "", "city, CA", EStateNameForm.Abbreviation);
            BasicTestFromLocation(",CALIFORNIA 84119,usA", "", "CA", "84119", "CA 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("  ,califorNia 84119", "", "CA", "84119", "CA 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city, california  ", "city", "CA", "", "city, CA", EStateNameForm.Abbreviation);
            BasicTestFromLocation("Salt        Lake City,    UTah     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.Abbreviation);
        }

        //Salt Lake City, UT 84111
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString2_WildZips()
        {
            BasicTestFromLocation("Salt Lake City, UT 84111 usa", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation(" city,ca 84119", "city", "CA", "84119", "city, CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("city,ca", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation("city ,ca   ", "city", "CA", "", "city, CA", EStateNameForm.NoPreference);
            BasicTestFromLocation(",ca 84119", "", "CA", "84119", "CA 84119", EStateNameForm.NoPreference);
            BasicTestFromLocation("  ,ca 90210 84119", "", "CA", "90210", "CA 90210", EStateNameForm.NoPreference);
            BasicTestFromLocation(" 841118city, cA  ", "city", "CA", "84111", "city, CA 84111", EStateNameForm.NoPreference);
            BasicTestFromLocation("Salt        Lake City,    UT     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.NoPreference);

            BasicTestFromLocation("Salt Lake City, UT 84111 , USA         ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);
            BasicTestFromLocation(" city,ca 84119", "city", "California", "84119", "city, California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city,ca", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation("city ,ca   ", "city", "California", "", "city, California", EStateNameForm.Full);
            BasicTestFromLocation(",ca 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("  ,ca 84119", "", "California", "84119", "California 84119", EStateNameForm.Full);
            BasicTestFromLocation("city, c84111A  ", "city", "California", "84111", "city, California 84111", EStateNameForm.Full);
            BasicTestFromLocation("Salt        Lake City,    UT     84111   ", "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111", EStateNameForm.Full);

            BasicTestFromLocation("Salt Lake City, Utah 84111 ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.Abbreviation);
            BasicTestFromLocation(" city,california 84119", "city", "CA", "84119", "city, CA 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city,California USA", "city", "CA", "", "city, CA", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city ,California   ", "city", "CA", "", "city, CA", EStateNameForm.Abbreviation);
            BasicTestFromLocation("841124,CALIFORNIA 84119,usA", "", "CA", "84112", "CA 84112", EStateNameForm.Abbreviation);
            BasicTestFromLocation("  ,califorNia 84119", "", "CA", "84119", "CA 84119", EStateNameForm.Abbreviation);
            BasicTestFromLocation("city, 22222 california  841118", "city", "CA", "22222", "city, CA 22222", EStateNameForm.Abbreviation);
            BasicTestFromLocation("Salt        Lake City,    UTah     84111   ", "Salt Lake City", "UT", "84111", "Salt Lake City, UT 84111", EStateNameForm.Abbreviation);
        }


        //Salt Lake City, UT
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString3()
        {
            BasicTestFromLocation("Denver, Colorado, USA", "Denver", "Colorado", "", "Denver, Colorado", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denver , Colorado usa", "Denver", "Colorado", "", "Denver, Colorado", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denver  ,           Colorado ", "Denver", "Colorado", "", "Denver, Colorado", EStateNameForm.NoPreference);
            BasicTestFromLocation("S L C, Colorado", "S L C", "Colorado", "", "S L C, Colorado", EStateNameForm.NoPreference);
        }

        //Salt Lake City, UT
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString3_WildZips()
        {
            BasicTestFromLocation("Denver, Colorado, USA 90210", "Denver", "Colorado", "90210", "Denver, Colorado 90210", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denver , 33333Colorado usa", "Denver", "Colorado", "33333", "Denver, Colorado 33333", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denver  33333,           Colorado ", "Denver", "Colorado", "33333", "Denver, Colorado 33333", EStateNameForm.NoPreference);
            BasicTestFromLocation("S33333 L C, Colorado", "S L C", "Colorado", "33333", "S L C, Colorado 33333", EStateNameForm.NoPreference);
        }

        //Salt Lake City 84111
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString4()
        {
            BasicTestFromLocation("Denver 88888, USA", "Denver", "", "88888", "Denver 88888", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denver 77777 usa", "Denver", "", "77777", "Denver 77777", EStateNameForm.NoPreference);
            BasicTestFromLocation(" DEnver     77777", "DEnver", "", "77777", "DEnver 77777", EStateNameForm.NoPreference);
            BasicTestFromLocation("S L C 11111", "S L C", "", "11111", "S L C 11111", EStateNameForm.NoPreference);

        }

        //Salt Lake City 84111
        [Fact]
        public void FromLocationString4_WildZips()
        {
            BasicTestFromLocation("Denver 88888, USA 66666", "Denver", "", "88888", "Denver 88888", EStateNameForm.NoPreference);
            BasicTestFromLocation(" Denv66666er 77777 usa", "Denver", "", "66666", "Denver 66666", EStateNameForm.NoPreference);
            BasicTestFromLocation(" 66666DEnver     77777", "DEnver", "", "66666", "DEnver 66666", EStateNameForm.NoPreference);
            BasicTestFromLocation("S L C 66666 11111", "S L C", "", "66666", "S L C 66666", EStateNameForm.NoPreference);

        }

        //84111
        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString5()
        {
            BasicTestFromLocation("11111", "", "", "11111", "11111", EStateNameForm.NoPreference);
            BasicTestFromLocation(" 11111 ", "", "", "11111", "11111", EStateNameForm.NoPreference);
            BasicTestFromLocation("11111         ", "", "", "11111", "11111", EStateNameForm.NoPreference);
            BasicTestFromLocation("        11111", "", "", "11111", "11111", EStateNameForm.NoPreference);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void FromLocationString5_WildZips()
        {
            BasicTestFromLocation("11111 22222", "", "", "11111", "11111", EStateNameForm.NoPreference);
            BasicTestFromLocation("22222 11111 ", "", "", "22222", "22222", EStateNameForm.NoPreference);
            BasicTestFromLocation("511111  33   11    ", "", "", "51111", "51111", EStateNameForm.NoPreference);
            BasicTestFromLocation("        111118", "", "", "11111", "11111", EStateNameForm.NoPreference);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void FromParts1()
        {
            var cityStateZip = CityStateZipBuilder.Build("Salt Lake City", "Utah", "84111");
            BasicTest(cityStateZip, "Salt Lake City", "Utah", "84111", "Salt Lake City, Utah 84111");


            cityStateZip = CityStateZipBuilder.Build("Salt Lake City", "Utah");
            BasicTest(cityStateZip, "Salt Lake City", "Utah", "", "Salt Lake City, Utah");

            cityStateZip = CityStateZipBuilder.Build("Salt Lake City", "Utah", null);
            BasicTest(cityStateZip, "Salt Lake City", "Utah", "", "Salt Lake City, Utah");

            //this is actually using the full location constructor of city builder
            cityStateZip = CityStateZipBuilder.Build("Salt Lake City", EStateNameForm.NoPreference);
            BasicTest(cityStateZip, "Salt Lake City", "", "", "Salt Lake City");

            cityStateZip = CityStateZipBuilder.Build("Salt Lake City", null);
            BasicTest(cityStateZip, "Salt Lake City", "", "", "Salt Lake City");

            cityStateZip = CityStateZipBuilder.Build("Salt Lake City", null, null);
            BasicTest(cityStateZip, "Salt Lake City", "", "", "Salt Lake City");
        }




        private void BasicTestFromLocation(string location, string expectedCity, string expectedState, string expectedZip, string expectedLocation, EStateNameForm stateNameFormOverride)
        {
            var cityStateZip = CityStateZipBuilder.Build(location, stateNameFormOverride);
            BasicTest(cityStateZip, expectedCity, expectedState, expectedZip, expectedLocation);
        }

        private void BasicTest(ICityStateZipWithString cityStateZip, string expectedCity, string expectedState, string expectedZip, string expectedLocation)
        {
            Assert.Equal(expectedCity, cityStateZip.City);
            Assert.Equal(expectedState, cityStateZip.State);
            Assert.Equal(expectedZip, cityStateZip.Zip);
            Assert.Equal(expectedLocation, cityStateZip.FullAddress);
        }


    }
}


