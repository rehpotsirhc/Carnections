using System;
using System.Collections.Generic;
using Common.Interfaces;
using System.Linq;

namespace VehicleData.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {

        VehicleDbContext _dbContext;

        public VehicleRepository(VehicleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IVehicleMinimal GetVehicle(string make, string model)
        {
            return _dbContext.Set<IVehicleMinimal>().Where(v =>
             String.Equals(v.Make == make, StringComparison.CurrentCultureIgnoreCase) &&
             String.Equals(v.Model == model, StringComparison.CurrentCultureIgnoreCase)).Single();
        }
        public IEnumerable<IVehicleMinimal> GetAll()
        {
            return _dbContext.Set<IVehicleMinimal>();
        }
    }
}
