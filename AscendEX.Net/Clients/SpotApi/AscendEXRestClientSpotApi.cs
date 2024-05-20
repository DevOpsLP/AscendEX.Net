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
    
  public async Task<WebCallResult<IEnumerable<Symbol>>> GetSymbolsAsync(CancellationToken ct = new CancellationToken())
    {
        var assets = await ExchangeData.GetProductsAsync("cash",ct: ct).ConfigureAwait(false);
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

    public Task<WebCallResult<Ticker>> GetTickerAsync(string symbol, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<WebCallResult<IEnumerable<Ticker>>> GetTickersAsync(CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
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
    public async Task<WebCallResult<Order>> GetOrderAsync(string orderId, string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        var o = await Trading.GetSingleOrderAsync(orderId, ct).ConfigureAwait(false);;

        return o.As(new Order
        {
            SourceObject = o.Data,
            Id = o.Data.Id.ToString(),
            Timestamp = o.Data.CreatedAt,
            Symbol = o.Data.ProductId,
            Side = o.Data.Side == OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
            Price = o.Data.Price,
            Quantity = o.Data.Size,
            QuantityFilled = o.Data.FilledSize,
            Type = GetOrderType(o.Data.Type),
            Status = GetOrderStatus(o.Data.Status)
        });
    }

    public Task<WebCallResult<IEnumerable<UserTrade>>> GetOrderTradesAsync(string orderId, string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<WebCallResult<IEnumerable<Order>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        var orderInfo = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
        if (!orderInfo)
            return orderInfo.As<IEnumerable<Order>>(null);

        return orderInfo.As(orderInfo.Data.Select(s =>
            new Order
            {
                SourceObject = s,
                Id = s.Id.ToString(),
                Symbol = s.ProductId,
                Side = s.Side == Enums.OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Price = s.Price,
                Quantity = s.Size,
                QuantityFilled = s.FilledSize,
                Type = GetOrderType(s.Type),
                Status = GetOrderStatus(s.Status),
                Timestamp = s.CreatedAt
            }));
    }
    
    private static CommonOrderType GetOrderType(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Limit => CommonOrderType.Limit,
            OrderType.Market => CommonOrderType.Market,
            _ => CommonOrderType.Other
        };
    }

    private static OrderType GetOrderType(CommonOrderType orderType)
    {
        return orderType switch
        {
            CommonOrderType.Limit => OrderType.Limit,
            CommonOrderType.Market => OrderType.Market,
            CommonOrderType.Other => OrderType.Stop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static CommonOrderStatus GetOrderStatus(Enums.OrderStatus orderStatus)
    {
        switch (orderStatus)
        {
            case OrderStatus.Open:
            case OrderStatus.Active:
                return CommonOrderStatus.Active;
            case OrderStatus.Done:
                return CommonOrderStatus.Filled;
            case OrderStatus.Pending:
            case OrderStatus.Rejected:
            case OrderStatus.Received:
            case OrderStatus.All:
            default:
                return CommonOrderStatus.Canceled;
        }
    }
    
    private OrderSide GetOrderSide(CommonOrderSide side)
    {
        return side switch
        {
            CommonOrderSide.Buy => OrderSide.Buy,
            CommonOrderSide.Sell => OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
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

    public async Task<WebCallResult<OrderId>> CancelOrderAsync(string orderId, string? symbol = null, CancellationToken ct = new CancellationToken())
    {
        var order = await Trading.CancelOrderAsync(orderId, ct).ConfigureAwait(false);;
        
        var orderResult = order.As(new OrderId
        {
            SourceObject = order,
            Id = order.Data.ToString(CultureInfo.InvariantCulture)
        });

        InvokeOrderCanceled(orderResult.Data);
        return orderResult;
    }


    public async Task<WebCallResult<OrderId>> PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price = null,
        string? accountId = null, string? clientOrderId = null, CancellationToken ct = new CancellationToken())
    {
        var order = await Trading.CreateOrderAsync(symbol, 
            GetOrderSide(side), 
            GetOrderType(type), 
            quantity, price.GetValueOrDefault()).ConfigureAwait(false);
            
        if(!order)
            return order.As<OrderId>(null);

        return order.As(new OrderId
        {
            SourceObject = order,
            Id = order.Data.Id.ToString()
        });
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
}