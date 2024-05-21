using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXOrderResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public OrderData Data { get; set; }

        public class OrderData
        {
            [JsonProperty("ac")]
            public string AccountCategory { get; set; }

            [JsonProperty("accountId")]
            public string AccountId { get; set; }

            [JsonProperty("action")]
            public string Action { get; set; }

            [JsonProperty("info")]
            public OrderInfo Info { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public class OrderInfo
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("orderId")]
            public string OrderId { get; set; }

            [JsonProperty("orderType")]
            public string OrderType { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }

            [JsonProperty("side")]
            public string Side { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("orderQty")]
            public string OrderQty { get; set; }

            [JsonProperty("cumFilledQty")]
            public string CumFilledQty { get; set; }
        }
    }
}
