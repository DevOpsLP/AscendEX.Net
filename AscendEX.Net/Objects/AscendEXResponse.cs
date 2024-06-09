namespace AscendEX.Net.Objects.Sockets
{
    public class AscendEXResponse<T>
    {
        public string M { get; set; }
        public string? Id { get; set; }
        public string Action { get; set; }
        public T Data { get; set; }
        public AscendEXError? Error { get; set; }
    }

    public class AscendEXError
    {
        public string M { get; set; }
        public string Id { get; set; }
        public long Code { get; set; }
        public string Reason { get; set; }
        public string? Info { get; set; }
    }
}
