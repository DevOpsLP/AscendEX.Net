using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXDepthResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public DepthData Data { get; set; }

        public class DepthData
        {
            [JsonProperty("m")]
            public string MessageType { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("data")]
            public DepthSnapshot Snapshot { get; set; }
        }

        public class DepthSnapshot
        {
            [JsonProperty("seqnum")]
            public long SequenceNumber { get; set; }

            [JsonProperty("ts")]
            public long Timestamp { get; set; }

            [JsonProperty("asks")]
            public List<List<string>> Asks { get; set; }

            [JsonProperty("bids")]
            public List<List<string>> Bids { get; set; }
        }
    }
}
