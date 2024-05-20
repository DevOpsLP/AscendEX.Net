using Newtonsoft.Json;

namespace AscendEX.Net.Objects.Internal;

public class AscendEXSocketRequest
{
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("product_ids")]
    public IEnumerable<string> ProductIds { get; set; }
    [JsonProperty("channels")]
    public IEnumerable<string> Channels { get; set; }
}