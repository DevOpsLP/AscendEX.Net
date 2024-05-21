using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXOrderStatusResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("accountCategory")]
        public string AccountCategory { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("data")]
        public List<OrderData> Data { get; set; }

        public class OrderData
        {
            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("orderQty")]
            public string OrderQty { get; set; }

            [JsonProperty("orderType")]
            public string OrderType { get; set; }

            [JsonProperty("avgPx")]
            public string AvgPx { get; set; }

            [JsonProperty("cumFee")]
            public string CumFee { get; set; }

            [JsonProperty("cumFilledQty")]
            public string CumFilledQty { get; set; }

            [JsonProperty("errorCode")]
            public string ErrorCode { get; set; }

            [JsonProperty("feeAsset")]
            public string FeeAsset { get; set; }

            [JsonProperty("lastExecTime")]
            public long LastExecTime { get; set; }

            [JsonProperty("orderId")]
            public string OrderId { get; set; }

            [JsonProperty("seqNum")]
            public long SeqNum { get; set; }

            [JsonProperty("side")]
            public string Side { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("stopPrice")]
            public string StopPrice { get; set; }

            [JsonProperty("execInst")]
            public string ExecInst { get; set; }
        }
    }
}
