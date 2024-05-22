using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXCancelOrder
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public CancelOrderData Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
        public class CancelOrderData
        {
            [JsonProperty("ac")]
            public string AccountCategory { get; set; }

            [JsonProperty("accountId")]
            public string AccountId { get; set; }

            [JsonProperty("action")]
            public string Action { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }

            [JsonProperty("info")]
            public OrderInfo Info { get; set; }

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
            }
        }
    }
}
