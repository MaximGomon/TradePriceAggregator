using MediatR;
using PriceAggregator.Common.Processor.Models;

namespace PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;

public class ReadCandleClosePriceCommand : ReadDataRequest, IRequest<List<CandleClosePrice>>
{
}