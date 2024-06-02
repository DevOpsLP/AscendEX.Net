using System;

namespace AscendEX.Net.Objects.Models
{
    public class AscendEXResponse<T>
    {
        public string M { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string Ac { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public T Info { get; set; } = default!;
    }

    public class AscendEXPlacedOrder
    {
        public string Symbol { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public long Timestamp { get; set; }
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
