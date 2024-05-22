using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXOrderHistoryResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<OrderHistoryData> Data { get; set; }

        public class OrderHistoryData
        {
            [JsonProperty("orderId")]
            public string OrderId { get; set; }

            [JsonProperty("seqNum")]
            public long SeqNum { get; set; }

            [JsonProperty("accountId")]
            public string AccountId { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("orderType")]
            public string OrderType { get; set; }

            [JsonProperty("side")]
            public string Side { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("stopPrice")]
            public string StopPrice { get; set; }

            [JsonProperty("orderQty")]
            public string OrderQty { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("createTime")]
            public long CreateTime { get; set; }

            [JsonProperty("lastExecTime")]
            public long LastExecTime { get; set; }

            [JsonProperty("avgFillPrice")]
            public string AvgFillPrice { get; set; }

            [JsonProperty("fillQty")]
            public string FillQty { get; set; }

            [JsonProperty("fee")]
            public string Fee { get; set; }

            [JsonProperty("feeAsset")]
            public string FeeAsset { get; set; }
        }
    }
}
