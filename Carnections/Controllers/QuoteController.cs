using CentralDispatchData.Repositories;
using Common.Interfaces;
using Enums.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quote;
using System.Threading.Tasks;

namespace Carnections.Controllers
{
    public class QuoteController
    {

        private IMemoryCache _CDListingsMapCache;
        private ICDListingRepository _CDListingsRepository;

        public QuoteController(IMemoryCache cache, ICDListingRepository repository)
        {
            _CDListingsMapCache = cache;
            _CDListingsRepository = repository;
        }

        public async Task<IActionResult> CalculateQuote(ILonLat pickup, ILonLat delivery, ETrailerType myTrailerType, bool myIsOperable, EVehicleType myVehicleType)
        {
            return new OkObjectResult((await CDTransportMap.GetMap(_CDListingsMapCache, _CDListingsRepository)).Search(pickup, delivery).CalculatePrice(myTrailerType, myIsOperable, myVehicleType));
        }

    }
}
