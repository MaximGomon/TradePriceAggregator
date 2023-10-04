using Microsoft.Net.Http.Headers;
using PriceAggregator.Common.Processor;
using PriceAggregator.Data.Context;
using PriceAggregator.Data.Context.Extensions;
using PriceAggregator.Data.Context.Seeds;
using TradePriceAggregator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTradeDbContext(builder.Configuration);
builder.Services.AddProcessors();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MigrateDbContext<TradeContext>((_, services) =>
{
    var logger = services.GetService<ILogger<TradeContext>>();

    new DataSourcesSeed()
        .SeedAsync(_, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(20)
        };
    context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

    await next();
});

app.Run();
