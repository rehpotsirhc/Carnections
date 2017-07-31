using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeCentralDispatch.Models
{
    public class CDListings
    {
        public int Count { get; set; }
        public int PageStart { get; set; }
        public IList<CDListing> Listings { get; set; }
    }
    public class CDListing
    { 
        public Location Pickup { get; set; }
        public Location Delivery { get; set; }
        public int ListingId { get; set; }
        public double Price { get; set; }
        public double PricePerMile { get; set; }
        public bool VehicleOperable { get; set; }
        public TrailerType ShipMethod { get; set; }
        [JsonProperty("vehicle_types")]
        public string VehicleTypes { get; set; }
        public string TruckMiles { get; set; }
        [JsonProperty("locations_valid")]
        public bool LocationsValid { get; set; }
    }
}

//        {  
//   "pickup":{  
//      "latitude":"41.852981",
//      "longitude":"-72.736005",
//      "city":"bloomfield",
//      "state":"CT",
//      "stateName":"",
//      "country":"",
//      "metro":"Hartford - West Hartford - East Hartford",
//      "zip":"06002",
//      "sloppy":false,
//      "citySearchMiles":"",
//      "mapUrl":"http:\/\/classic.mapquest.com\/embed?zoom=5&q=bloomfield+CT+06002",
//      "milesAndBearing":"91mi SW"
//   },
//   "delivery":{  
//      "latitude":"33.614684",
//      "longitude":"-84.061082",
//      "city":"conyers",
//      "state":"GA",
//      "metro":"Atlanta - Sandy Springs - Marietta",
//      "zip":"30094",
//      "stateName":"",
//      "country":"",
//      "sloppy":false,
//      "citySearchMiles":"",
//      "mapUrl":"http:\/\/classic.mapquest.com\/embed?zoom=5&q=conyers+GA+30094",
//      "milesAndBearing":"25mi SE"
//   },
//   "listingId":"45160603",
//   "alphaId":"nKO2UBPKUU4Cw3rn",
//   "company":"DIY Transport, Inc.",
//   "orderId":"5666463",
//   "modifiedDate":"07\/17\/17",
//   "shipBeginDate":"08\/09\/17",
//   "vehicles":"(SUV)",
//   "price":"400",
//   "phone":"330-208-2528",
//   "locations_valid":true,
//   "additional_info":"",
//   "vehicle_make":"2004 honda pilot",
//   "vehicle_types":"(SUV)",
//   "truckMiles":"1022",
//   "pricePerMile":"0.39",
//   "numVehicles":"1",
//   "timezone":"EST",
//   "hours":"24\/7",
//   "rating":"<strong>Rating:<\/strong> <a href='javascript:void(0)'\n            onclick=\"window.open('\/protected\/rating\/client-snapshot?id=nKO2UBPKUU4Cw3rn#ratings', 'ratingPopup');\">\n            100.0%<\/a>, received: 2274, member: 11\/14",
//   "rating_license":"valid",
//   "rating_score":"100.0%",
//   "rating_count":"2274",
//   "vehicleOperable":true,
//   "shipMethod":"Open",
//   "unknownType":false,
//   "formattedCompany":"DIY Transport, Inc.",
//   "wideLoad":0,
//   "composite":"<div class='vehicleBlock'>2004 Honda Pilot<span class='2004-Honda-Pilot'> <span class='glyphicon glyphicon-info-sign' data-toggle='popover' data-html='true' data-vehicleYear='2004' data-make='honda' data-model='pilot' data-vin=''><\/span><\/span> <\/div><div class='vehicleSummary'>(SUV)<\/div><span class=\"abnormalConditions\"><\/span>",
//   "newListing":false,
//   "comparable":true,
//   "priceText":"400 COD (Cash\/Certified)",
//   "routeHref":"http:\/\/classic.mapquest.com\/embed?q1=bloomfield+CT+06002+US&q2=conyers+GA+30094+US",
//   "vehiclesCount":1,
//   "preferred_shipper":false,
//   "fax":"(440) 744-2096"
//}

//public int NumVehicles { get; set; }
//public int VehiclesCount { get; set; }