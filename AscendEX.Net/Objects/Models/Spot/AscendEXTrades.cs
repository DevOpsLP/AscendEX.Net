using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXTradesResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public TradesData Data { get; set; }

        public class TradesData
        {
            [JsonProperty("m")]
            public string MessageType { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("data")]
            public List<Trade> Trades { get; set; }
        }

        public class Trade
        {
            [JsonProperty("seqnum")]
            public long SequenceNumber { get; set; }

            [JsonProperty("p")]
            public string Price { get; set; }

            [JsonProperty("q")]
            public string Quantity { get; set; }

            [JsonProperty("ts")]
            public long Timestamp { get; set; }

            [JsonProperty("bm")]
            public bool IsBuyerMaker { get; set; }
        }
    }
}
