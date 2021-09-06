using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SeriousChallenge.Infrastructure.Abstractions;
using SeriousChallenge.Models;
using SeriousChallengeApi.ControllerServices;
using SeriousChallengeApi.Dtos;
using SeriousChallengeApi.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SeriousChallengeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StockSymbolController : ControllerBase
    {
        private readonly IStockSymbolControllerService _service;
        private readonly ILogger<StockSymbolController> _logger;

        public StockSymbolController(IStockSymbolControllerService service, ILogger<StockSymbolController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("Compare")]
        public async Task<ActionResult> Compare([FromBody] string symbolName)
        {
            try
            {
                var result = await _service.Compare(symbolName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Operation is failed is exception: {ex}");
                throw;
            }
        }
    }
}
