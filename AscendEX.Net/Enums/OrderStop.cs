using CryptoExchange.Net.Attributes;

namespace AscendEX.Net.Enums;

/// <summary>
/// The side of an order
/// </summary>
public enum OrderStop
{
    /// <summary>
    /// Loss
    /// </summary>
    [Map("loss")]
    Loss,
    /// <summary>
    /// Entry
    /// </summary>
    [Map("entry")]
    Entry
}