using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXCashBalance
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<CashBalanceData> Data { get; set; }

        public class CashBalanceData
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("totalBalance")]
            public string TotalBalance { get; set; }

            [JsonProperty("availableBalance")]
            public string AvailableBalance { get; set; }
        }
    }
}
