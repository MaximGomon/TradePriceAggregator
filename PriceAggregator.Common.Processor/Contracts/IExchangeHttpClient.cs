namespace PriceAggregator.Common.Processor.Contracts;

public interface IExchangeHttpClient<in TParam, TResult> where TResult : class
{
    Task<TResult> MakeCall(HttpClient client, TParam parameter);
}