using Common.Interfaces;
using GoogleDistance;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carnections.Controllers
{
    public class LocationLookupController
    {

        public async Task<IActionResult> LookupCityStateZip(ICityStateZip cityStatezip)
        {
            return new OkObjectResult(await LookupLonLat.Lookup(cityStatezip));
        }
    }
}
