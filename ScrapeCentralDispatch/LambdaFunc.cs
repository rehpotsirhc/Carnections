using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ScrapeCentralDispatch
{
    public class LambdaFunc
    {
        public async Task ScrapeCDAndSave(bool deleteOld, ILambdaContext context)
        {

            if (Environment.DO_MIGRATION)
                DbServicesCollection.MigrationDb();


            //   DbServicesCollection.Repository.DeleteOld(Environment.SAVE_RECORD_FOR_DAYS + 1, Environment.DATABASE_BATCH_SIZE);

            await ScrapeCD.ScrapeAll();

            //wait for the first batch to complete so we know how many records exist, so we know when to stop
            //var firstBatch = ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, i, Environment.SCRAPE_BATCH_SIZE).GetListings().Result;
            //int absoluteTotal = 30000;
            //Console.WriteLine($"Total number of records: {absoluteTotal}");
            //  yield return firstBatch;

            //var tasks = new List<Task>();
            //for (int i = 0; i < 3000; i += Environment.SCRAPE_BATCH_SIZE)
            //{
            //    tasks.Add(ScrapeCD.BuildEndpointAll(Environment.MIN_PER_MILE, i, Environment.SCRAPE_BATCH_SIZE).GetListings());
            //    Console.WriteLine(i);

            //    //    .ContinueWith(completed =>
            //    //{
            //    //    switch (completed.Status)
            //    //    {
            //    //        case TaskStatus.RanToCompletion: DbServicesCollection.Repository.AddorUpdate(JsonConvert.DeserializeObject<CDListingCollection>(completed.Result).Listings.Select(l => CDListingTransformer.Transform(l)), Environment.DATABASE_BATCH_SIZE); break;
            //    //            //  case TaskStatus.Faulted: onBatchScrapeError(completed.Exception.InnerException); break;

            //    //    }
            //    //}, TaskContinuationOptions.RunContinuationsAsynchronously);


            //}

            //await Task.WhenAll(tasks);

        }
    }
}
