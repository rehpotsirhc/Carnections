using Microsoft.Extensions.Configuration;
using System.IO;


namespace ScrapeCentralDispatch
{
    public class Environment
    {
        static Environment()
        {
            var config = new ConfigurationBuilder()
               // .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            CONNECTION_STRING = Common.Utils.EnvironmentVars.GetStringVariable("connectionString", config);
            DO_MIGRATION = Common.Utils.EnvironmentVars.GetBooleanVariable("doMigration", config);
            SAVE_RECORD_FOR_DAYS = Common.Utils.EnvironmentVars.GetIntVariable("saveRecordForDays", config);
            MIN_PER_MILE = Common.Utils.EnvironmentVars.GetDoubleVariable("minPerMile", config);
            SCRAPE_BATCH_SIZE = Common.Utils.EnvironmentVars.GetIntVariable("scrapeBatchSize", config);
            DATABASE_BATCH_SIZE = Common.Utils.EnvironmentVars.GetIntVariable("databaseBatchSize", config);
            INITIAL_TOTAL = SCRAPE_BATCH_SIZE + 1;
        }
        public static string CONNECTION_STRING { get; set; }
        public static bool DO_MIGRATION { get; set; }
        public static int SAVE_RECORD_FOR_DAYS { get; set; }
        public static double MIN_PER_MILE { get; set; }
        public static int SCRAPE_BATCH_SIZE { get; set; }
        public static int DATABASE_BATCH_SIZE { get; set; }
        public static int INITIAL_TOTAL { get; }
    }
}
