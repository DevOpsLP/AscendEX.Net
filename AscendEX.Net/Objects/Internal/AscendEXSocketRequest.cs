namespace AscendEX.Net.Objects.Internal
{
    public class AscendEXSocketRequest
    {
        public string Op { get; set; }
        public Dictionary<string, string>[] Args { get; set; }
    }
}
