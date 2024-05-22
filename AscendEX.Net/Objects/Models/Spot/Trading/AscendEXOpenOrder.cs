using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXOpenOrdersResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<OrderInfo> Data { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        public class OrderInfo
        {
            [JsonProperty("orderId")]
            public string OrderId { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("side")]
            public string Side { get; set; }

            [JsonProperty("orderType")]
            public string OrderType { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("orderQty")]
            public string OrderQty { get; set; }

            [JsonProperty("cumFilledQty")]
            public string CumFilledQty { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("avgPx")]
            public string AvgPx { get; set; }

            [JsonProperty("cumFee")]
            public string CumFee { get; set; }

            [JsonProperty("feeAsset")]
            public string FeeAsset { get; set; }

            [JsonProperty("lastExecTime")]
            public long LastExecTime { get; set; }

            [JsonProperty("seqNum")]
            public long SeqNum { get; set; }

            [JsonProperty("stopPrice")]
            public string StopPrice { get; set; }

            [JsonProperty("execInst")]
            public string ExecInst { get; set; }
        }
    }
}
