namespace AscendEX.Net.Objects.Sockets
{
    public class AscendEXSocketRequest
    {
        public string Op { get; set; }
        public string Id { get; set; }
        public string Action { get; set; }
        public string? Account { get; set; }
        public Dictionary<string, object>? Args { get; set; }
    }
}
