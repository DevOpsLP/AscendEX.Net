using System.Globalization;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

namespace AscendEX.Net.Clients.SpotApi;

/// <inheritdoc cref="IAscendEXSpotApi" />
public class AscendEXRestClientSpotApi : RestApiClient, IAscendEXRestClientSpotApi, ISpotClient
{
    #region fields 
    /// <inheritdoc />
    public new AscendEXRestApiOptions ApiOptions => (AscendEXRestApiOptions)base.ApiOptions;
    /// <inheritdoc />
    public new AscendEXRestOptions ClientOptions => (AscendEXRestOptions)base.ClientOptions;
    #endregion

    #region Api clients
    /// <inheritdoc />
    public IAscendEXRestClientSpotApiExchangeData ExchangeData { get; }
    public IAscendEXRestClientSpotApiTrading Trading { get; }
    public IAscendEXRestClientSpotApiAccount Account { get; }
    public IAscendEXRestClientSpotApiWallet Wallet { get; }

    public string ExchangeName => "AscendEX";
    #endregion

    private static OrderSide GetOrderSide(CommonOrderSide side)
    {
        return side switch
        {
            CommonOrderSide.Buy => OrderSide.Buy,
            CommonOrderSide.Sell => OrderSide.Sell,
            _ => throw new ArgumentException("Unsupported order side for AscendEX order: " + side)
        };
    }
    private static CommonOrderSide GetCommonOrderSide(string side)
    {
        return side.ToLower() switch
        {
            "buy" => CommonOrderSide.Buy,
            "sell" => CommonOrderSide.Sell,
            _ => throw new ArgumentException("Unsupported order side for AscendEX order: " + side)
        };
    }


    private static CommonOrderType GetCommonOrderType(Enums.OrderType type)
    {
        return type switch
        {
            Enums.OrderType.Limit => CommonOrderType.Limit,
            Enums.OrderType.Market => CommonOrderType.Market,
            Enums.OrderType.StopMarket => CommonOrderType.Other, // Adjust if there's a better mapping
            Enums.OrderType.StopLimit => CommonOrderType.Other,  // Adjust if there's a better mapping
            _ => throw new ArgumentException("Unsupported order type for AscendEX order: " + type)
        };
    }

    private static CommonOrderStatus GetCommonOrderStatus(Enums.OrderStatus status)
    {
        return status switch
        {
            Enums.OrderStatus.New => CommonOrderStatus.Active,
            Enums.OrderStatus.PartiallyFilled => CommonOrderStatus.Filled,
            Enums.OrderStatus.Filled => CommonOrderStatus.Filled,
            Enums.OrderStatus.Canceled => CommonOrderStatus.Canceled,
            Enums.OrderStatus.PendingNew => CommonOrderStatus.Active,
            Enums.OrderStatus.Rejected => CommonOrderStatus.Canceled,
            Enums.OrderStatus.Accept => CommonOrderStatus.Filled,
            Enums.OrderStatus.Ack => CommonOrderStatus.Active,
            Enums.OrderStatus.Done => CommonOrderStatus.Filled,
            Enums.OrderStatus.Err => CommonOrderStatus.Canceled,
            Enums.OrderStatus.Active => CommonOrderStatus.Active,
            Enums.OrderStatus.All => CommonOrderStatus.Filled, // Assuming 'All' implies completion
            Enums.OrderStatus.Open => CommonOrderStatus.Active,
            Enums.OrderStatus.Received => CommonOrderStatus.Filled,
            Enums.OrderStatus.Pending => CommonOrderStatus.Active,
            _ => throw new ArgumentException("Unsupported order status for AscendEX order: " + status)
        };
    }
    private static Enums.OrderType GetOrderType(CommonOrderType type)
    {
        return type switch
        {
            CommonOrderType.Limit => Enums.OrderType.Limit,
            CommonOrderType.Market => Enums.OrderType.Market,
            CommonOrderType.Other => Enums.OrderType.StopMarket, // Adjust if there's a better mapping
            _ => throw new ArgumentException("Unsupported order type for AscendEX order: " + type)
        };
    }

    public AscendEXRestClientSpotApi(ILogger logger, HttpClient? httpClient, AscendEXRestOptions options)
        : base(logger, httpClient, options.Environment.SpotAndMarginRestAddress, options, options.SpotAndMarginOptions)
    {
        ExchangeData = new AscendEXRestClientSpotApiExchangeData(logger, this);
        Trading = new AscendEXRestClientSpotApiTrading(logger, this);
        Account = new AscendEXRestClientSpotApiAccount(logger, this);
        Wallet = new AscendEXRestClientSpotApiWallet(logger, this);  // Add this line
    }

    internal Uri GetUrl(string endpoint)
    {
        return new Uri(BaseAddress.AppendPath(endpoint));
    }

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
    {
        // Ensure credentials are of the correct type
        if (credentials is AscendEXApiCredentials ascendexCredentials)
            return new AscendEXAuthenticationProvider(ascendexCredentials);

        throw new ArgumentException("Invalid credentials provided. Expected AscendEXApiCredentials.", nameof(credentials));
    }
    public override TimeSyncInfo? GetTimeSyncInfo() => null;

    public override TimeSpan? GetTimeOffset() => null;

    public string GetSymbolName(string baseAsset, string quoteAsset)
    {
        return $"{baseAsset}/{quoteAsset}".ToUpper();
    }


    public async Task<WebCallResult<OrderId>> PlaceOrderAsync(
        string symbol,
        CommonOrderSide side,
        CommonOrderType type,
        decimal quantity,
        decimal? price,
        string? accountId,
        string? clientOrderId,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException(nameof(symbol) + " required for AscendEX " + nameof(ISpotClient.PlaceOrderAsync), nameof(symbol));

        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException(nameof(accountId) + " required for AscendEX " + nameof(ISpotClient.PlaceOrderAsync), nameof(accountId));

        // Assuming accountId is a composite of accountGroup and accountCategory
        var accountParts = accountId.Split(':');
        if (accountParts.Length != 2)
            throw new ArgumentException("Invalid accountId format. Expected format is 'accountGroup:accountCategory'", nameof(accountId));

        if (!int.TryParse(accountParts[0], out var accountGroup))
            throw new ArgumentException("Invalid accountGroup in accountId", nameof(accountId));

        var accountCategory = accountParts[1];

        var priceStr = price?.ToString();
        var stopPriceStr = (decimal?)null; // Replace with actual stop price if needed

        var order = await Trading.PlaceOrderAsync(
            accountGroup,
            accountCategory,
            symbol,
            GetOrderSide(side),
            GetOrderType(type),
            quantity,
            priceStr,
            clientOrderId,
            stopPriceStr?.ToString(CultureInfo.InvariantCulture),
            type == CommonOrderType.Limit ? "GTC" : null, // Assuming timeInForce for limit orders is Good Till Canceled
            null, // respInst is null for now, can be adjusted as needed
            ct).ConfigureAwait(false);

        if (!order)
            return order.As<OrderId>(null);

        return order.As(new OrderId
        {
            SourceObject = order,
        });
    }


    public async Task<WebCallResult<IEnumerable<Symbol>>> GetSymbolsAsync(CancellationToken ct = new CancellationToken())
    {
        var assets = await ExchangeData.GetProductsAsync("cash", ct: ct).ConfigureAwait(false);
        if (!assets)
            return assets.As<IEnumerable<Symbol>>(null);

        return assets.As(assets.Data.Data.Select(s =>
            new Symbol()
            {
                SourceObject = s,
                Name = s.DisplayName,
                MinTradeQuantity = Convert.ToDecimal(s.MinQty)
            }));
    }

    public async Task<WebCallResult<Ticker>> GetTickerAsync(string symbol, CancellationToken ct = new CancellationToken())
    {
        var tickerResult = await ExchangeData.GetTickerAsync(symbol, ct).ConfigureAwait(false);

        if (!tickerResult.Success || tickerResult.Data == null || !tickerResult.Data.Data.Any())
            return tickerResult.As<Ticker>(null);

        var firstTicker = tickerResult.Data.Data.First();

        var ticker = new Ticker
        {
            Symbol = firstTicker.Symbol,
            HighPrice = decimal.TryParse(firstTicker.High, NumberStyles.Any, CultureInfo.InvariantCulture, out var highPrice) ? highPrice : (decimal?)null,
            LowPrice = decimal.TryParse(firstTicker.Low, NumberStyles.Any, CultureInfo.InvariantCulture, out var lowPrice) ? lowPrice : (decimal?)null,
            LastPrice = decimal.TryParse(firstTicker.Close, NumberStyles.Any, CultureInfo.InvariantCulture, out var lastPrice) ? lastPrice : (decimal?)null,
            Volume = decimal.TryParse(firstTicker.Volume, NumberStyles.Any, CultureInfo.InvariantCulture, out var volume) ? volume : (decimal?)null,
            // Assuming QuoteVolume and PriceChange information is available or can be derived
            // QuoteVolume = , 
            // PriceChange = ,
            // PriceChangePercent = 
        };

        return new WebCallResult<Ticker>(
            tickerResult.ResponseStatusCode,
            tickerResult.ResponseHeaders,
            tickerResult.ResponseTime,
            tickerResult.ResponseLength,
            tickerResult.OriginalData,
            tickerResult.RequestUrl,
            tickerResult.RequestBody,
            tickerResult.RequestMethod,
            tickerResult.RequestHeaders,
            ticker,
            tickerResult.Error);
    }

    public async Task<WebCallResult<IEnumerable<Ticker>>> GetTickersAsync(CancellationToken ct = new CancellationToken())
    {
        var tickersResult = await ExchangeData.GetTickersAsync(ct).ConfigureAwait(false);

        if (!tickersResult.Success || tickersResult.Data == null || !tickersResult.Data.Data.Any())
            return new WebCallResult<IEnumerable<Ticker>>(
                tickersResult.ResponseStatusCode,
                tickersResult.ResponseHeaders,
                tickersResult.ResponseTime,
                tickersResult.ResponseLength,
                tickersResult.OriginalData,
                tickersResult.RequestUrl,
                tickersResult.RequestBody,
                tickersResult.RequestMethod,
                tickersResult.RequestHeaders,
                null,
                tickersResult.Error);

        var tickers = tickersResult.Data.Data.Select(t => new Ticker
        {
            Symbol = t.Symbol,
            HighPrice = decimal.TryParse(t.High, NumberStyles.Any, CultureInfo.InvariantCulture, out var highPrice) ? highPrice : (decimal?)null,
            LowPrice = decimal.TryParse(t.Low, NumberStyles.Any, CultureInfo.InvariantCulture, out var lowPrice) ? lowPrice : (decimal?)null,
            LastPrice = decimal.TryParse(t.Close, NumberStyles.Any, CultureInfo.InvariantCulture, out var lastPrice) ? lastPrice : (decimal?)null,
            Volume = decimal.TryParse(t.Volume, NumberStyles.Any, CultureInfo.InvariantCulture, out var volume) ? volume : (decimal?)null,
            // Assuming QuoteVolume and PriceChange information is available or can be derived
            // QuoteVolume = , 
            // PriceChange = ,
            // PriceChangePercent = 
        }).ToList();

        return new WebCallResult<IEnumerable<Ticker>>(
            tickersResult.ResponseStatusCode,
            tickersResult.ResponseHeaders,
            tickersResult.ResponseTime,
            tickersResult.ResponseLength,
            tickersResult.OriginalData,
            tickersResult.RequestUrl,
            tickersResult.RequestBody,
            tickersResult.RequestMethod,
            tickersResult.RequestHeaders,
            tickers,
            tickersResult.Error);
    }

    public Task<WebCallResult<IEnumerable<Kline>>> GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null,
        int? limit = null, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<WebCallResult<OrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<WebCallResult<IEnumerable<Trade>>> GetRecentTradesAsync(string symbol, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }
    public async Task<WebCallResult<Order>> GetOrderAsync(string orderId, string? accountId, CancellationToken ct = new CancellationToken())
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ArgumentException(nameof(orderId) + " required for AscendEX " + nameof(ISpotClient.GetOrderAsync), nameof(orderId));

        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException(nameof(accountId) + " required for AscendEX " + nameof(ISpotClient.GetOrderAsync), nameof(accountId));

        // Assuming accountId is a composite of accountGroup and accountCategory
        var accountParts = accountId.Split(':');
        if (accountParts.Length != 2)
            throw new ArgumentException("Invalid accountId format. Expected format is 'accountGroup:accountCategory'", nameof(accountId));

        var accountGroup = accountParts[0];
        var accountCategory = accountParts[1];

        var orderResult = await Trading.GetSingleOrderAsync(accountGroup, accountCategory, orderId, ct).ConfigureAwait(false);

        if (!orderResult.Success || orderResult.Data == null || !orderResult.Data.Data.Any())
            return new WebCallResult<Order>(
                orderResult.ResponseStatusCode,
                orderResult.ResponseHeaders,
                orderResult.ResponseTime,
                orderResult.ResponseLength,
                orderResult.OriginalData,
                orderResult.RequestUrl,
                orderResult.RequestBody,
                orderResult.RequestMethod,
                orderResult.RequestHeaders,
                null,
                orderResult.Error);

        var firstOrderData = orderResult.Data.Data.First();

        var order = new Order
        {
            Id = firstOrderData.OrderId,
            Symbol = firstOrderData.Symbol,
            Side = GetCommonOrderSide(firstOrderData.Side),
            Type = GetCommonOrderType((Enums.OrderType)Enum.Parse(typeof(Enums.OrderType), firstOrderData.OrderType, true)),
            Price = decimal.TryParse(firstOrderData.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : (decimal?)null,
            Quantity = decimal.TryParse(firstOrderData.OrderQty, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantity) ? quantity : (decimal?)null,
            QuantityFilled = decimal.TryParse(firstOrderData.CumFilledQty, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantityFilled) ? quantityFilled : (decimal?)null,
            Status = GetCommonOrderStatus((Enums.OrderStatus)Enum.Parse(typeof(Enums.OrderStatus), firstOrderData.Status, true)),
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(firstOrderData.LastExecTime).DateTime
        };

        return new WebCallResult<Order>(
            orderResult.ResponseStatusCode,
            orderResult.ResponseHeaders,
            orderResult.ResponseTime,
            orderResult.ResponseLength,
            orderResult.OriginalData,
            orderResult.RequestUrl,
            orderResult.RequestBody,
            orderResult.RequestMethod,
            orderResult.RequestHeaders,
            order,
            orderResult.Error);
    }

    public Task<WebCallResult<IEnumerable<UserTrade>>> GetOrderTradesAsync(string orderId, string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }
   public async Task<WebCallResult<IEnumerable<Order>>> GetOpenOrdersAsync(string? accountId, CancellationToken ct = new CancellationToken())
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException(nameof(accountId) + " required for AscendEX " + nameof(GetOpenOrdersAsync), nameof(accountId));

        // Assuming accountId is a composite of accountGroup and accountCategory
        var accountParts = accountId.Split(':');
        if (accountParts.Length != 2)
            throw new ArgumentException("Invalid accountId format. Expected format is 'accountGroup:accountCategory'", nameof(accountId));

        if (!int.TryParse(accountParts[0], out var accountGroup))
            throw new ArgumentException("Invalid accountGroup in accountId", nameof(accountId));

        var accountCategory = accountParts[1];

        var openOrdersResult = await Trading.GetOpenOrdersAsync(accountGroup, accountCategory, null, ct).ConfigureAwait(false);

        if (!openOrdersResult.Success)
        {
            // Handle error
            Console.WriteLine($"Failed to fetch open orders: {openOrdersResult.Error}");
            return openOrdersResult.As<IEnumerable<Order>>(null);
        }

        var orders = openOrdersResult.Data.Data.Select(orderData => new Order
        {
            Id = orderData.OrderId,
            Symbol = orderData.Symbol,
            Side = GetCommonOrderSide(orderData.Side),
            Type = GetCommonOrderType((Enums.OrderType)Enum.Parse(typeof(Enums.OrderType), orderData.OrderType, true)),
            Price = decimal.TryParse(orderData.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : (decimal?)null,
            Quantity = decimal.TryParse(orderData.OrderQty, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantity) ? quantity : (decimal?)null,
            QuantityFilled = decimal.TryParse(orderData.CumFilledQty, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantityFilled) ? quantityFilled : (decimal?)null,
            Status = GetCommonOrderStatus((Enums.OrderStatus)Enum.Parse(typeof(Enums.OrderStatus), orderData.Status, true)),
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(orderData.LastExecTime).DateTime
        });

        return openOrdersResult.As(orders);
    }

    public event Action<OrderId>? OnOrderPlaced;
    public event Action<OrderId>? OnOrderCanceled;

    internal void InvokeOrderPlaced(OrderId id)
    {
        OnOrderPlaced?.Invoke(id);
    }

    internal void InvokeOrderCanceled(OrderId id)
    {
        OnOrderCanceled?.Invoke(id);
    }

    public Task<WebCallResult<IEnumerable<Order>>> GetClosedOrdersAsync(string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
        Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
        ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
    {
        var result = await SendRequestAsync<T>(uri,
            method, cancellationToken, parameters, signed,
            postPosition, arraySerialization, weight,
            additionalHeaders: new Dictionary<string, string>
            {
                { "User-Agent", Guid.NewGuid().ToString() }
            },
            ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        return result;
    }

    public Task<WebCallResult<IEnumerable<Balance>>> GetBalancesAsync(string? accountId = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public async Task<WebCallResult<AscendEXCancelAllOrders>> CancelAllOrdersAsync(string accountId, string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        // Assuming accountId is a composite of accountGroup and accountCategory
        var accountParts = accountId.Split(':');
        if (accountParts.Length != 2)
            throw new ArgumentException("Invalid accountId format. Expected format is 'accountGroup:accountCategory'", nameof(accountId));

        if (!int.TryParse(accountParts[0], out var accountGroup))
            throw new ArgumentException("Invalid accountGroup in accountId", nameof(accountId));

        var accountCategory = accountParts[1];

        var result = await Trading.CancelAllOrdersAsync(accountGroup, accountCategory, symbol, ct).ConfigureAwait(false);

        if (!result.Success)
        {
            // Check if the response contains an error message
            var errorData = result.Data;
            if (!string.IsNullOrEmpty(errorData.Message))
            {
                Console.WriteLine($"Order cancellation failed: {errorData.Message}");
            }
            else
            {
                Console.WriteLine($"Order cancellation failed with code: {errorData.Code}");
            }
        }

        return result;
    }

    public Task<WebCallResult<OrderId>> CancelOrderAsync(string orderId, string? symbol = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}