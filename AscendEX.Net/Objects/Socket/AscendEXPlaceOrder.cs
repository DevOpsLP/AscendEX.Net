using System;

namespace AscendEX.Net.Objects.Models
{
    public class AscendEXPlaceOrderRequest
    {
        public string Op { get; set; } = "req";
        public string Action { get; set; } = "place-order";
        public string? Id { get; set; }
        public string? Account { get; set; }
        public AscendEXOrderArgs Args { get; set; } = new AscendEXOrderArgs();
    }

    public class AscendEXOrderArgs
    {
        public long Time { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Symbol { get; set; } = string.Empty;
        public string OrderPrice { get; set; } = string.Empty;
        public string OrderQty { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public bool PostOnly { get; set; }
        public string RespInst { get; set; } = "ACK";
    }
}
