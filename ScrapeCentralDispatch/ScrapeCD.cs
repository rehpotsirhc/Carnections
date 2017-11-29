using CentralDispatchData.Repositories;
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
using CentralDispatchData;
using Common.Models;
using System.Threading;

namespace ScrapeCentralDispatch
{
    //Perhaps this method should not be static and instead be an instance class with a backing interface
    public static class ScrapeCD
    {



        // code from
        // https://blogs.msdn.microsoft.com/pfxteam/2012/08/02/processing-tasks-as-they-complete/
        //public static void ScrapeAllAndSave(
        //    Action<Func<IAsyncEnumerable<Task<string>>>, Action<string>, Action<Exception>> scrapeAllAndSaveConcurrently = null,
        //    Func<IAsyncEnumerable<Task<string>>> scrape = null,
        //    Action<string> onBatchScrapeCompleted = null, Action<Exception> onBatchScrapeError = null)
        //{
        //    //need to try/test Method1 and Method2
        //    scrapeAllAndSaveConcurrently = scrapeAllAndSaveConcurrently ?? RunTasksConcurrently_Method1;
        //    scrape = scrape ?? ScrapeAll;
        //    onBatchScrapeCompleted = onBatchScrapeCompleted ?? OnBatchScrapeCompleted;
        //    onBatchScrapeError = onBatchScrapeError ?? OnBatchScrapeError;

        //    scrapeAllAndSaveConcurrently(scrape, onBatchScrapeCompleted, onBatchScrapeError);


        //}

        private static void RunTasksConcurrently_Method1(Func<IAsyncEnumerable<Task<string>>> scrape,
            Action<string> onBatchScrapeCompleted, Action<Exception> onBatchScrapeError)
        {

            scrape().ForEach(batch =>
             batch.ContinueWith(completed =>
             {
                 switch (completed.Status)
                 {
                     case TaskStatus.RanToCompletion: onBatchScrapeCompleted(completed.Result); break;
                     case TaskStatus.Faulted: onBatchScrapeError(completed.Exception.InnerException); break;

                 }
             }, TaskContinuationOptions.RunContinuationsAsynchronously)

            );

            //foreach (Task<ICDListingCollection> listingBatchTask in scrape().ToAsyncEnumerable())
            //{

            //}
        }

        //private static async void RunTasksConcurrently_Method2(ICDListingRepository<TransformedListing> repository, Func<IEnumerable<Task<ICDListingCollection>>> scrape,
        //Action<ICDListingRepository<TransformedListing>, ICDListingCollection> onBatchScrapeCompleted, Action<Exception> onBatchScrapeError)
        //{
        //    IEnumerable<Task<ICDListingCollection>> tasks = scrape();
        //    while (tasks.Count > 0)
        //    {
        //        var t = await Task.WhenAny(tasks);
        //        tasks.Remove(t);
        //        try { onBatchScrapeCompleted(repository, await t); }
        //        catch (OperationCanceledException) { }
        //        catch (Exception e) { onBatchScrapeError(e); }
        //    }
        //}

        private static void OnBatchScrapeCompleted(string listingsBatch)
        {
            //Utilizes .NET dependency injection to recreate the repository and context for each batch
            DbServicesCollection.Repository.AddorUpdate(JsonConvert.DeserializeObject<CDListingCollection>(listingsBatch).Listings.Select(l => CDListingTransformer.Transform(l)), Environment.DATABASE_BATCH_SIZE);
        }
        private static void OnBatchScrapeError(Exception e)
        {
            Console.WriteLine("Batch Scrape Exception: ");
            Console.WriteLine(e);
        }


        public static async Task ScrapeAll()
        {
            //wait for the first batch to complete so we know how many records exist, so we know when to stop
            //var firstBatch = ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, i, Environment.SCRAPE_BATCH_SIZE).GetListings().Result;
            //int absoluteTotal = 30000;
            //Console.WriteLine($"Total number of records: {absoluteTotal}");
            //  yield return firstBatch;

            var tasks = new List<Task<string>>();
            for (int i = 0; i < 150; i += Environment.SCRAPE_BATCH_SIZE)
            {
                Thread.Sleep(1000);
                tasks.Add(ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, i, Environment.SCRAPE_BATCH_SIZE).GetListings());



                Console.WriteLine(i);

            }

            var dbTasks= new List<Task>();
            tasks.ForEach(batch =>
             dbTasks.Add(batch.ContinueWith(completed =>
              {
                  switch (completed.Status)
                  {
                      case TaskStatus.RanToCompletion:
                          {
                              ICDListingCollection listings;
                              try
                              {

                                  // return JsonConvert.DeserializeObject<CentralDispatchListings>(response, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                                  listings = JsonConvert.DeserializeObject<CDListingCollection>(completed.Result);
                                  Console.WriteLine("listings deserialized");
                              }
                              catch (Exception)
                              {
                                  listings = null;
                                  Console.WriteLine("listings null");
                              }
                              if(listings != null)
                                DbServicesCollection.Repository.AddorUpdate(listings.Listings.Select(l => CDListingTransformer.Transform(l)), Environment.DATABASE_BATCH_SIZE); break;
                          }
                          //  case TaskStatus.Faulted: onBatchScrapeError(completed.Exception.InnerException); break;

                  }
              }, TaskContinuationOptions.RunContinuationsAsynchronously)));

           await Task.WhenAll(dbTasks);



            // yield break return batches.ToAsyncEnumerable();
        }

        public static async Task<string> GetListings(this string endpoint)
        {
            return await HttpGet.Get(endpoint, "Cookie", BuildCookieHeader());

            //await Task.Delay(5000);

            //if (response == null)
            //    return null;
            //try
            //{

            //    // return JsonConvert.DeserializeObject<CentralDispatchListings>(response, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            //    return JsonConvert.DeserializeObject<CDListingCollection>(response);
            //}
            //catch (Exception)
            //{
            //    return null;
            //}
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

        public static string BuildEndpointAll(double minPerMile, int pageStart, int listingsPerPage)
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
            // sb.AppendFormat("{0}={1}", "PHPSESSID", "90de1b4bdc671d655474aebf413a632a"); //I think I locked this one out. We probably need a thread.sleep in between requests to prevent this
            sb.AppendFormat("{0}={1}", "PHPSESSID", "c1350653c8049b8096127a45466a9f60");
            return sb.ToString();
        }
    }
}
