using MediatR;
using PriceAggregator.Common.Processor.Models;

namespace PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;

public class ReadCandleClosePriceCommand : IRequest<CandleClosePrice>
{
    public DateTime Time { get; set; }
    public string Candle { get; set; }
}