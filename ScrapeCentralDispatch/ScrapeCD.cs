using CentralDispatchData.Models;
using CentralDispatchData.Repositories;
using Common.Interfaces;
using Common.Models;
using Common.Utils;
using GoogleDistance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeCentralDispatch
{
    public static class ScrapeCD
    {

        private static int perPage = 500;
        private static int Total = perPage;

        public static void ScrapeAllAndSave(CDListingRepository repository)
        {
            foreach(Task<ICDListings> listingBatchTask in ScrapeAll())
            {
                ICDListings listingBatch = listingBatchTask.Result;
                Total = listingBatch.Count;
                repository.AddorUpdate(listingBatch.Listings, 250, true);
            }
        }

        public static IEnumerable<Task<ICDListings>> ScrapeAll()
        {
            double minPerMile = .1;
            for (int t = 0, i = 0; t < Total; t += perPage, i++)
            {
                yield return ScrapeCD.BuildEndpointAll(minPerMile, t, perPage).GetListings();
            }
        }

        public static async Task<ICDListings> GetListings(this string endpoint)
        {
            string response = await HttpGet.Get(endpoint, "Cookie", BuildCookieHeader());
            if (response == null)
                return null;
            try
            {
                // return JsonConvert.DeserializeObject<CentralDispatchListings>(response, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                return JsonConvert.DeserializeObject<CDListings>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static string BuildEndpoint(ICityStateZip pickup, int pickupRadius, ICityStateZip delivery, int deliveryRadius, TrailerType trailerType, bool operable, int shipWithin, double minPerMile)
        {

            // string condition = operable ? "1" : "0";
            var sb = new StringBuilder();
            sb.Append("https://www.centraldispatch.com/protected/listing-search/get-results-ajax?").Append("&");
            sb.AppendFormat("{0}={1}", "FatAllowCanada", "0").Append("&");
            sb.AppendFormat("{0}={1}", "pickupCitySearch", "1").Append("&");
            sb.AppendFormat("{0}={1}", "pickupRadius", pickupRadius.ToString()).Append("&");
            sb.AppendFormat("{0}={1}", "pickupCity", WebUtility.UrlEncode(pickup.City)).Append("&");
            sb.AppendFormat("{0}={1}", "pickupState", WebUtility.UrlEncode(StateBuilder.GetAbbreviation(pickup.State))).Append("&");
            sb.AppendFormat("{0}={1}", "pickupZip", pickup.Zip).Append("&");
            sb.AppendFormat("{0}={1}", "origination_valid", "1").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryCitySearch", "1").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryRadius", deliveryRadius.ToString()).Append("&");
            sb.AppendFormat("{0}={1}", "deliveryCity", WebUtility.UrlEncode(delivery.City)).Append("&");
            sb.AppendFormat("{0}={1}", "deliveryState", WebUtility.UrlEncode(StateBuilder.GetAbbreviation(delivery.State))).Append("&");
            sb.AppendFormat("{0}={1}", "deliveryZip", delivery.Zip).Append("&");
            sb.AppendFormat("{0}={1}", "destination_valid", "1").Append("&");
            sb.AppendFormat("{0}={1}", "trailerType", "").Append("&");
            sb.AppendFormat("{0}={1}", "vehiclesRun", "").Append("&"); //?
            sb.AppendFormat("{0}={1}", "minVehicles", "1").Append("&");
            sb.AppendFormat("{0}={1}", "maxVehicles", "").Append("&");
            sb.AppendFormat("{0}={1}", "shipWithin", shipWithin.ToString()).Append("&");
            sb.AppendFormat("{0}={1}", "minPayPrice", "").Append("&");
            sb.AppendFormat("{0}={1}", "minPayPerMile", minPerMile.ToString()).Append("&");
            sb.AppendFormat("{0}={1}", "highlightPeriod", "0").Append("&");
            sb.AppendFormat("{0}={1}", "listingsPerPage", "100").Append("&");
            sb.AppendFormat("{0}={1}", "postedBy", "").Append("&");
            sb.AppendFormat("{0}={1}", "primarySort", "1").Append("&");
            sb.AppendFormat("{0}={1}", "secondarySort", "4");
            return sb.ToString();
        }

        private static string BuildEndpointAll(double minPerMile, int pageStart, int listingsPerPage)
        {
            var sb = new StringBuilder();
            sb.Append("https://www.centraldispatch.com/protected/listing-search/get-results-ajax?").Append("&");
            //   sb.AppendFormat("{0}={1}", "FatAllowCanada", "0").Append("&");
            sb.AppendFormat("{0}={1}", "pickupAreas[]", "All").Append("&");
            sb.AppendFormat("{0}={1}", "pickupCitySearch", "0").Append("&");
            sb.AppendFormat("{0}={1}", "pickupRadius", "25").Append("&");
            sb.AppendFormat("{0}={1}", "pickupCity", "").Append("&");
            sb.AppendFormat("{0}={1}", "pickupState", "").Append("&");
            sb.AppendFormat("{0}={1}", "pickupZip", "").Append("&");
            sb.AppendFormat("{0}={1}", "origination_valid", "1").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryAreas[]", "All").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryCitySearch", "0").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryRadius", "25").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryCity", "").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryState", "").Append("&");
            sb.AppendFormat("{0}={1}", "deliveryZip", "").Append("&");
            sb.AppendFormat("{0}={1}", "destination_valid", "1").Append("&");
            sb.AppendFormat("{0}={1}", "trailerType", "").Append("&");
            sb.AppendFormat("{0}={1}", "vehiclesRun", "").Append("&"); //?
            sb.AppendFormat("{0}={1}", "minVehicles", "1").Append("&");
            sb.AppendFormat("{0}={1}", "maxVehicles", "").Append("&");
            sb.AppendFormat("{0}={1}", "shipWithin", "60").Append("&");
            sb.AppendFormat("{0}={1}", "minPayPrice", "").Append("&");
            sb.AppendFormat("{0}={1}", "minPayPerMile", minPerMile.ToString()).Append("&");
            sb.AppendFormat("{0}={1}", "highlightPeriod", "0").Append("&");
            sb.AppendFormat("{0}={1}", "listingsPerPage", listingsPerPage).Append("&");
            sb.AppendFormat("{0}={1}", "postedBy", "").Append("&");
            sb.AppendFormat("{0}={1}", "primarySort", "1").Append("&");
            sb.AppendFormat("{0}={1}", "secondarySort", "4").Append("&");
            //sb.AppendFormat("{0}={1}", "pageSize", "1000").Append("&");
            sb.AppendFormat("{0}={1}", "pageStart", pageStart);
            return sb.ToString();
        }

        private static string BuildCookieHeader()
        {
            /*
             * visitedDashboard, _gid, PHPSESSID are required
             */

            var sb = new StringBuilder();
            string sep = "; ";
            sb.AppendFormat("{0}={1}", "visitedDashboard", "1").Append(sep);
            //sb.AppendFormat("{0}={1}", "defaultView", "1").Append(sep); //this was listed twice?
            //sb.AppendFormat("{0}={1}", "__utmt=", "1").Append(sep);
            //sb.AppendFormat("{0}={1}", "__utma", "262503198.138626099.1495316764.1495557398.1495557398.1").Append(sep);
            //sb.AppendFormat("{0}={1}", "__utmb", "262503198.10.10.1495557398").Append(sep);
            //sb.AppendFormat("{0}={1}", "__utmc", "262503198").Append(sep);
            //sb.AppendFormat("{0}={1}", "__utmz", "262503198.1495557398.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)").Append(sep);
            //sb.AppendFormat("{0}={1}", "_gat", "1").Append(sep);
            //sb.AppendFormat("{0}={1}", "__ga", "GA1.2.138626099.1495316764").Append(sep);
            sb.AppendFormat("{0}={1}", "_gid", "GA1.2.1877014367.1495558286").Append(sep);
            //sb.AppendFormat("{0}={1}", "test-persistent", "1").Append(sep);
            //sb.AppendFormat("{0}={1}", "test-session", "1").Append(sep); //again, twice
            sb.AppendFormat("{0}={1}", "PHPSESSID", "90de1b4bdc671d655474aebf413a632a");

            return sb.ToString();
        }
    }
}
