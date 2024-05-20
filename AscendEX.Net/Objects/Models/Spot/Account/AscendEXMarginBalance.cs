using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXMarginBalance
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<MarginBalanceData> Data { get; set; }

        public class MarginBalanceData
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("totalBalance")]
            public string TotalBalance { get; set; }

            [JsonProperty("availableBalance")]
            public string AvailableBalance { get; set; }

            [JsonProperty("borrowed")]
            public string Borrowed { get; set; }

            [JsonProperty("interest")]
            public string Interest { get; set; }
        }
    }
}
