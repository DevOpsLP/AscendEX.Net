using System;
using System.Text;

public static class OrderIdGenerator
{
    public static string GenerateOrderId(long timestamp, string userUID, string clientOrderId = null)
    {
        // Convert timestamp to hex string
        string timestampHex = timestamp.ToString("X");

        // Ensure userUID is in the correct format
        if (string.IsNullOrEmpty(userUID) || userUID.Length != 11 || !userUID.StartsWith("U"))
            throw new ArgumentException("Invalid userUID");

        // Generate or truncate clientOrderId
        string clientOrderIdPart;
        if (!string.IsNullOrEmpty(clientOrderId) && clientOrderId.Length >= 9)
        {
            clientOrderIdPart = clientOrderId.Substring(clientOrderId.Length - 9);
        }
        else
        {
            clientOrderIdPart = GenerateRandomString(9);
        }

        // Concatenate parts to form orderId
        string orderId = $"a{timestampHex}{userUID}{clientOrderIdPart}";
        return orderId;
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}
