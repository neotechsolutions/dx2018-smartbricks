using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TravelSearchContracts;

namespace TravelSearchBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : ControllerBase
    {
        private readonly DataStore _store;

        public TravelsController(IHostingEnvironment hostingEnvironment, IMemoryCache cache)
        {
            _store = new DataStore(hostingEnvironment, cache);
        }

        [HttpGet("")]
        [HttpGet("all")]
        public Task<List<Travel>> GetAllTravels()
        {
            return _store.GetAllTravelsAsync();
        }

        [HttpPost("search")]
        public Task<List<Travel>> SearchTravels([FromBody] SearchQuery query)
        {
            return _store.SearchTravelsAsync(query);
        }
    }
}
