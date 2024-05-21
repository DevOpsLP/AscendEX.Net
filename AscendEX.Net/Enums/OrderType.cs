using CryptoExchange.Net.Attributes;

namespace AscendEX.Net.Enums;

public enum OrderType
{
    /// <summary>
    /// Limit
    /// </summary>
    [Map("limit")]
    Limit,
    /// <summary>
    /// Market
    /// </summary>
    [Map("market")]
    Market,
    /// <summary>
    /// Stop Market
    /// </summary>
    [Map("stop_market")]
    StopMarket,
    /// <summary>
    /// Stop Limit
    /// </summary>
    [Map("stop_limit")]
    StopLimit
}
