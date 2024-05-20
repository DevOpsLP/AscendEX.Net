using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXMarginRisk
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public MarginRiskData Data { get; set; }

        public class MarginRiskData
        {
            [JsonProperty("accountMaxLeverage")]
            public string AccountMaxLeverage { get; set; }

            [JsonProperty("availableBalanceInUSDT")]
            public string AvailableBalanceInUSDT { get; set; }

            [JsonProperty("totalBalanceInUSDT")]
            public string TotalBalanceInUSDT { get; set; }

            [JsonProperty("totalBorrowedInUSDT")]
            public string TotalBorrowedInUSDT { get; set; }

            [JsonProperty("totalInterestInUSDT")]
            public string TotalInterestInUSDT { get; set; }

            [JsonProperty("netBalanceInUSDT")]
            public string NetBalanceInUSDT { get; set; }

            [JsonProperty("pointsBalance")]
            public string PointsBalance { get; set; }

            [JsonProperty("currentLeverage")]
            public string CurrentLeverage { get; set; }

            [JsonProperty("cushion")]
            public string Cushion { get; set; }
        }
    }
}
