using AscendEX.Net.Enums;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.JsonNet;

namespace AscendEX.Net.Converters
{
    internal class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new()
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "limit"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "market"),
            new KeyValuePair<OrderType, string>(OrderType.StopMarket, "stop_market"),
            new KeyValuePair<OrderType, string>(OrderType.StopLimit, "stop_limit")
        };
    }
}
