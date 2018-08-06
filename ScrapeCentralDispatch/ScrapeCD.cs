using Common.Interfaces;
using Common.Utils;
using Enums.Models;
using GoogleDistance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Common.Models;
using System.Threading;

namespace ScrapeCentralDispatch
{
    //Perhaps this method should not be static and instead be an instance class with a backing interface
    public static class ScrapeCD
    {
        // code from
        // https://blogs.msdn.microsoft.com/pfxteam/2012/08/02/processing-tasks-as-they-complete/
        public static void ScrapeAllAndSave(int waitBetweenRequests,
            Func<int, Func<int, List<Task<string>>>, Action<string>, Action<Exception>, List<Task<string>>> scrapeAllAndSaveConcurrently = null,
            Func<int, List<Task<string>>> scrape = null,
            Action<string> onBatchScrapeCompleted = null, Action<Exception> onBatchScrapeError = null)
        {
            //need to try/test Method1 and Method2
            scrapeAllAndSaveConcurrently = scrapeAllAndSaveConcurrently ?? RunTasksConcurrently_Method1;
            scrape = scrape ?? ScrapeAll;
            onBatchScrapeCompleted = onBatchScrapeCompleted ?? OnBatchScrapeCompleted;
            onBatchScrapeError = onBatchScrapeError ?? OnBatchScrapeError;

            var allScrapes = scrapeAllAndSaveConcurrently(waitBetweenRequests, scrape, onBatchScrapeCompleted, onBatchScrapeError);
            Task.WaitAll(allScrapes.ToArray());
        }

        private static List<Task<string>> RunTasksConcurrently_Method1(int waitBetweenRequests, Func<int, List<Task<string>>> scrape,
            Action<string> onBatchScrapeCompleted, Action<Exception> onBatchScrapeError)
        {
            var allScrapes = scrape(waitBetweenRequests);
            allScrapes.ForEach(batch =>
             batch.ContinueWith(completed =>
             {
                 switch (completed.Status)
                 {
                     case TaskStatus.RanToCompletion: onBatchScrapeCompleted(completed.Result); break;
                     case TaskStatus.Faulted: onBatchScrapeError(completed.Exception.InnerException); break;
                 }
             }, TaskContinuationOptions.RunContinuationsAsynchronously)

            );
            return allScrapes;
        }

        private static List<Task<string>> RunTasksConcurrently_Method2(int waitBetweenRequests, Func<int, List<Task<string>>> scrape,
            Action<string> onBatchScrapeCompleted, Action<Exception> onBatchScrapeError)
        {
            var allScrapes = scrape(waitBetweenRequests);
            while (allScrapes.Count > 0)
            {
                var t = Task.WhenAny(allScrapes).Result;
                allScrapes.Remove(t);
                try { onBatchScrapeCompleted(t.Result); }
                catch (OperationCanceledException) { }
                catch (Exception e) { onBatchScrapeError(e); }
            }
            return allScrapes;
        }

        private static void OnBatchScrapeCompleted(string listingsBatch)
        {
            var ICDListingCollection = DeserializeListingsBatch(listingsBatch);
            if(ICDListingCollection != null && ICDListingCollection.Listings.Count > 0)
                DbServicesCollection.Repository.AddorUpdate(ICDListingCollection.Listings.Select(l => CDListingTransformer.Transform(l)), Environment.DATABASE_BATCH_SIZE);
        }
        private static void OnBatchScrapeError(Exception e)
        {
            Console.WriteLine("Batch Scrape Exception: ");
            Console.WriteLine(e);
        }

        private static ICDListingCollection DeserializeListingsBatch(string listingsBatch)
        {
            try
            {
                return JsonConvert.DeserializeObject<CDListingCollection>(listingsBatch);
            }
            catch(Exception e)
            {
                return null;
            }
        }


        private static List<Task<string>> ScrapeAll(int waitBetweenRequests)
        {
            int pageStart = 0;
            int batchSize = Environment.SCRAPE_BATCH_SIZE;

            //wait until the first batch is complete to get the total number of records
            var firstBatch = DeserializeListingsBatch(ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, pageStart, batchSize).GetListings().Result);

            if(firstBatch == null)
            {
                //stop all scraping
            }

            int absoluteTotal = firstBatch.Count;

            var tasks = new List<Task<string>>();
            for (pageStart = 1; pageStart < absoluteTotal; pageStart += batchSize)
            {
                tasks.Add(ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, pageStart, batchSize).GetListings());
                Thread.Sleep(waitBetweenRequests);
            }

            //for the last batch, get the remaining
            pageStart -= Environment.SCRAPE_BATCH_SIZE;
            tasks.Add(ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, pageStart, batchSize).GetListings());

            return tasks;
        }

        private static async Task<string> GetListings(this string endpoint)
        {
            return await HttpUtils.GetAsString(endpoint, "Cookie", BuildCookieHeader());
        }

        private static string BuildEndpoint(ICityStateZip pickup, int pickupRadius, ICityStateZip delivery, int deliveryRadius, ETrailerType trailerType, bool operable, int shipWithin, double minPerMile)
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


        private static string phpSessId;
        private static string BuildCookieHeader()
        {
            /*
             * visitedDashboard and PHPSESSID are required
             */

            if (phpSessId == null)
                phpSessId = HttpUtils.LoginToCD("https://www.centraldispatch.com/login?uri=/protected/", "Bartd", "bj1234567", "d873a3798996b524ed173da1a2b2eebb6d19d1c2912672280eb6964d008d684c").Result;

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
            //sb.AppendFormat("{0}={1}", "_gid", "GA1.2.1877014367.1495558286").Append(sep);
            //sb.AppendFormat("{0}={1}", "test-persistent", "1").Append(sep);
            //sb.AppendFormat("{0}={1}", "test-session", "1").Append(sep); //again, twice
            // sb.AppendFormat("{0}={1}", "PHPSESSID", "90de1b4bdc671d655474aebf413a632a"); //I think I locked this one out. We probably need a thread.sleep in between requests to prevent this
            // sb.AppendFormat("{0}={1}", "PHPSESSID", "c1350653c8049b8096127a45466a9f60");
            //sb.AppendFormat("{0}={1}", "PHPSESSID", "bd3994d369818eee5b22da029e0a029a");
            sb.AppendFormat("{0}={1}", "PHPSESSID", "1a001ff182cd3925b0cc8030ff76d4e0");


            return sb.ToString();
        }
    }
}
