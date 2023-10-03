using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TradePriceAggregator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/prices")]
    public class PriceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PriceController> _logger;

        public PriceController(ILogger<PriceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }
    }
}