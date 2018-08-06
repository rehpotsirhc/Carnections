using Amazon.Lambda.Core;

namespace ScrapeCentralDispatch
{
    public class LambdaFunc
    {
        public void ScrapeCDAndSave(bool deleteOld, ILambdaContext context)
        {

            if (Environment.DO_MIGRATION)
                DbServicesCollection.MigrationDb();

           // DbServicesCollection.Repository.DeleteOld(Environment.SAVE_RECORD_FOR_DAYS + 1, Environment.DATABASE_BATCH_SIZE);

            ScrapeCD.ScrapeAllAndSave(1000);
        }
    }
}
