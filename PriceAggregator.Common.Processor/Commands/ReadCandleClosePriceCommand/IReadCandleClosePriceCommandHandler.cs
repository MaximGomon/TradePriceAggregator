using MediatR;
using PriceAggregator.Common.Processor.Models;

namespace PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;

public interface IReadCandleClosePriceCommandHandler : IRequestHandler<ReadCandleClosePriceCommand, List<CandleClosePrice>>
{

}