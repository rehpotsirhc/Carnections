using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CentralDispatchData.Repositories;
using CentralDispatchData;
using Common.Models;
using Common.Interfaces;

namespace ScrapeCentralDispatch
{
    public class DbServicesCollection
    {
        private static DbContextOptions<CDListingDbContext> _dbContextOptions;
        public static void MigrationDb()
        {
            int attempts = 3;
            while (attempts-- > 0)
            {
                try
                {
                    DbContext.Database.Migrate();
                    break;
                }

                catch (Exception)
                {

                }
            }
        }

        public static ICDListingRepository Repository
        {
            get
            {
                return new CDListingRepository(DbContext);
            }
        }
        public static CDListingDbContext DbContext
        {
            get
            {
                if (_dbContextOptions == null)
                {
                    _dbContextOptions = new DbContextOptionsBuilder<CDListingDbContext>()
                        .UseMySql(Environment.CONNECTION_STRING)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .EnableSensitiveDataLogging().Options;
                }
                return new CDListingDbContext(_dbContextOptions);
            }

        }
    }
}
