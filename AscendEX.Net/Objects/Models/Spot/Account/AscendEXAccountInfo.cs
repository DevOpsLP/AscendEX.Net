using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXAccountInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public AccountData Data { get; set; }

        public class AccountData
        {
            [JsonProperty("accountGroup")]
            public int AccountGroup { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("expireTime")]
            public long ExpireTime { get; set; }

            [JsonProperty("allowedIps")]
            public List<string> AllowedIps { get; set; }

            [JsonProperty("cashAccount")]
            public List<string> CashAccount { get; set; }

            [JsonProperty("marginAccount")]
            public List<string> MarginAccount { get; set; }

            [JsonProperty("userUID")]
            public string UserUID { get; set; }

            [JsonProperty("tradePermission")]
            public bool TradePermission { get; set; }

            [JsonProperty("transferPermission")]
            public bool TransferPermission { get; set; }

            [JsonProperty("viewPermission")]
            public bool ViewPermission { get; set; }
        }
    }
}
