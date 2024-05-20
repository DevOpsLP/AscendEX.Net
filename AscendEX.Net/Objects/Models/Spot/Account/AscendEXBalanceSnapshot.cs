using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXBalanceSnapshot
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("snapshot")]
        public List<Snapshot> Snapshots { get; set; }
    }

    public class Meta
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

    public class Snapshot
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("totalBalance")]
        public string TotalBalance { get; set; }
    }
}
