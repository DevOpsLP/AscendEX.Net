using CryptoExchange.Net.Attributes;

namespace AscendEX.Net.Enums;

/// <summary>
/// The side of an order
/// </summary>
public enum OrderSide
{
    /// <summary>
    /// Buy
    /// </summary>
    [Map("buy")]
    Buy,
    /// <summary>
    /// Sell
    /// </summary>
    [Map("sell")]
    Sell
}