public class SubscriptionRequest
{
    public string Op { get; set; }
    public string Id { get; set; }
    public string Ch { get; set; }

    public SubscriptionRequest(string op, string id, string ch)
    {
        Op = op;
        Id = id;
        Ch = ch;
    }
}
