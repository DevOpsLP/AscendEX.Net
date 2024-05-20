using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXProductResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<AscendEXProduct> Data { get; set; }
    }

    public class AscendEXProduct
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("tradingStartTime")]
        public long TradingStartTime { get; set; }

        [JsonProperty("collapseDecimals")]
        public string CollapseDecimals { get; set; }

        [JsonProperty("minQty")]
        public string MinQty { get; set; }

        [JsonProperty("maxQty")]
        public string MaxQty { get; set; }

        [JsonProperty("minNotional")]
        public string MinNotional { get; set; }

        [JsonProperty("maxNotional")]
        public string MaxNotional { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("tickSize")]
        public string TickSize { get; set; }

        [JsonProperty("useTick")]
        public bool UseTick { get; set; }

        [JsonProperty("lotSize")]
        public string LotSize { get; set; }

        [JsonProperty("useLot")]
        public bool UseLot { get; set; }

        [JsonProperty("commissionType")]
        public string CommissionType { get; set; }

        [JsonProperty("commissionReserveRate")]
        public string CommissionReserveRate { get; set; }

        [JsonProperty("qtyScale")]
        public int QtyScale { get; set; }

        [JsonProperty("priceScale")]
        public int PriceScale { get; set; }

        [JsonProperty("notionalScale")]
        public int NotionalScale { get; set; }
    }
}
