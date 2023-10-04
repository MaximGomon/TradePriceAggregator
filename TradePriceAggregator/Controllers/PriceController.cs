using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PriceAggregator.Common.Extensions;
using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;
using TradePriceAggregator.Models;

namespace TradePriceAggregator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/trades")]
    public class PriceController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IMediator _mediator;
        private readonly ILogger<PriceController> _logger;

        public PriceController(ILogger<PriceController> logger, 
            IMemoryCache cache, 
            IMediator mediator)
        {
            _logger = logger;
            _cache = cache;
            _mediator = mediator;
        }

        [HttpGet("{candle}/{time:int}/close")]
        [ProducesResponseType(typeof(CandleClosePrice), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTradePrice(CandleType candle, int time)
        {
            if (time <= 0)
                return BadRequest("Wrong timestamp");

            var dateTime = time.EpochToSystemTime();

            if (_cache.TryGetValue($"{candle}_{dateTime.ToEpochTime()}", out CandleClosePrice price))
            {
                return Ok(price);
            }

            var command = new ReadCandleClosePriceCommand()
            {
                Candle = candle.ToString(),
                Time = dateTime
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Data for this timestamp and candle was not found on the server");

            _cache.Set($"{candle}_{dateTime.ToEpochTime()}", result, dateTime.TillEndOfCurrentHour());

            return Ok(result);
        }
    }
}