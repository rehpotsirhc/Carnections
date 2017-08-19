using CentralDispatchData.Repositories;
using Common.Interfaces;
using Enums.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quote.CoordMeshMap;
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

        [HttpPost]
        public async Task<IActionResult> CalculateQuote([FromBody]IQuote quote)
        {
            return new OkObjectResult(await CalculateQuoteHelper(quote.Pickup, quote.Delivery, quote.TrailerType, quote.VehicleIsOperable, quote.Vehicle));
        }

        [HttpPost]
        public async Task<IActionResult> CalculateQuote([FromBody]ILocation pickup, [FromBody]ILocation delivery, [FromBody]ETrailerType trailerType, [FromBody] bool isOperable, [FromBody]IVehicleMinimal vehicleMinimal)
        {
            return new OkObjectResult(await CalculateQuoteHelper(pickup, delivery, trailerType, isOperable, vehicleMinimal));
        }

        private async Task<IPrice> CalculateQuoteHelper(ILocation pickup, ILocation delivery, ETrailerType trailerType, bool isOperable, IVehicleMinimal vehicleMinimal)
        {
            return (await CDTransportMap.GetMap(_CDListingsMapCache, _CDListingsRepository)).Search(pickup, delivery).CalculatePrice(trailerType, isOperable, vehicleMinimal);
        }
    }
}
