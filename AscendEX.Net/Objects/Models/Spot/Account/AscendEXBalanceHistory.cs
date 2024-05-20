using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXBalanceHistory
    {
        [JsonProperty("meta")]
        public BalanceHistoryMeta Meta { get; set; }

        [JsonProperty("snapshot")]
        public List<BalanceHistorySnapshot> Snapshot { get; set; }

        public class BalanceHistoryMeta
        {
            [JsonProperty("ac")]
            public string AccountCategory { get; set; }

            [JsonProperty("accountId")]
            public string AccountId { get; set; }

            [JsonProperty("sn")]
            public long SequenceNumber { get; set; }

            [JsonProperty("snapshotTime")]
            public long SnapshotTime { get; set; }
        }

        public class BalanceHistorySnapshot
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("totalBalance")]
            public string TotalBalance { get; set; }
        }
    }
}
