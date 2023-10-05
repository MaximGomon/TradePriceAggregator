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

        [HttpGet("{candle}/{start:int}/{end:int}/close")]
        [ProducesResponseType(typeof(List<CandleClosePrice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTradePrice(CandleType candle, int start, int end)
        {
            if (start <= 0 || end <= 0 || end <= start)
                return BadRequest("Wrong timestamp");

            var startTime = start.EpochToSystemTime();
            var endTime = end.EpochToSystemTime();
            var key = $"{candle}_{startTime.ToEpochTime()}_{endTime.ToEpochTime()}";

            if (_cache.TryGetValue(key, out List<CandleClosePrice> price))
            {
                return Ok(price);
            }

            var command = new ReadCandleClosePriceCommand()
            {
                Candle = candle.ToString(),
                Start = startTime,
                End = endTime
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Data for this timestamp and candle was not found on the server");

            var totalExpiration = startTime.TillEndOfCurrentHour();
            _cache.Set(key, result, totalExpiration);

            return Ok(result);
        }
    }
}