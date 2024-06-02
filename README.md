# AscendEX.Net Wrapper ReadMe

## Overview

AscendEX.Net is a .NET wrapper for the AscendEX API, created using the CryptoExchange.Net library. It provides a convenient and structured way to interact with AscendEX endpoints.

## Features

- Retrieve account information
- Fetch risk limit info
- Get exchange info latency
- Check cash and margin balances
- Assess margin risk
- Perform asset transfers
- Retrieve balance snapshots and history
- Get wallet deposit addresses

## Endpoints for "Account"

1. **Get Account Info**
   - **Endpoint:** `/api/pro/v1/info`
   - **Method:** `GET`
   - **Function:** `GetAccountInfoAsync`

2. **Get Risk Limit Info**
   - **Endpoint:** `/api/pro/v2/risk-limit-info`
   - **Method:** `GET`
   - **Function:** `GetRiskLimitInfoAsync`

3. **Get Exchange Info Latency**
   - **Endpoint:** `/api/pro/v1/exchange-info`
   - **Method:** `GET`
   - **Function:** `GetExchangeInfoAsync`
   - **Parameters:** `long requestTime`

4. **Get Cash Balance**
   - **Endpoint:** `/api/pro/v1/cash/balance`
   - **Method:** `GET`
   - **Function:** `GetCashBalanceAsync`
   - **Parameters:** `int accountGroup`

5. **Get Margin Balance**
   - **Endpoint:** `/api/pro/v1/margin/balance`
   - **Method:** `GET`
   - **Function:** `GetMarginBalanceAsync`
   - **Parameters:** `int accountGroup`

6. **Get Margin Risk**
   - **Endpoint:** `/api/pro/v1/margin/risk`
   - **Method:** `GET`
   - **Function:** `GetMarginRiskAsync`
   - **Parameters:** `int accountGroup`

7. **Transfer Assets**
   - **Endpoint:** `/api/pro/v1/transfer`
   - **Method:** `POST`
   - **Function:** `TransferAsync`
   - **Parameters:** `int accountGroup`, `AscendEXTransferRequest request`

8. **Get Balance Snapshot**
   - **Endpoint:** `/api/pro/data/v1/{accountCategory}/balance/snapshot`
   - **Method:** `GET`
   - **Function:** `GetBalanceSnapshotAsync`
   - **Parameters:** `string accountCategory`, `string date`

9. **Get Balance History**
   - **Endpoint:** `/api/pro/data/v1/{accountCategory}/balance/history`
   - **Method:** `GET`
   - **Function:** `GetBalanceHistoryAsync`
   - **Parameters:** `string accountCategory`, `string date`
   
## ExchangeData Functions and Endpoints

### GetCurrenciesAsync
- **Endpoint:** `/api/pro/v2/assets`
- **HTTP Method:** GET
- **Parameters:** None
- **Description:** Retrieves a list of all currencies supported by the exchange.

### GetProductsAsync
- **Endpoint:** `/api/pro/v1/{accountCategory}/products`
- **HTTP Method:** GET
- **Parameters:**
  - `accountCategory` (string): The category of the account (e.g., `cash`, `margin`).
- **Description:** Retrieves a list of all products for the specified account category.

### GetTickerAsync
- **Endpoint:** `/api/pro/v1/spot/ticker`
- **HTTP Method:** GET
- **Parameters:**
  - `symbol` (optional, string): The symbol to fetch the ticker for (e.g., `BTC/USDT`). If not provided, fetches tickers for all symbols.
- **Description:** Retrieves the ticker information for the specified symbol or all symbols if none is provided.

### GetTickersAsync
- **Endpoint:** `/api/pro/v1/spot/ticker`
- **HTTP Method:** GET
- **Parameters:** None
- **Description:** Retrieves the ticker information for all symbols.

### GetBarHistInfoAsync
- **Endpoint:** `/api/pro/v1/barhist/info`
- **HTTP Method:** GET
- **Parameters:** None
- **Description:** Retrieves information about historical bar intervals.

### GetBarHistAsync
- **Endpoint:** `/api/pro/v1/barhist`
- **HTTP Method:** GET
- **Parameters:**
  - `symbol` (string, required): The trading pair symbol (e.g., `ASD/USDT`).
  - `interval` (string, required): The interval for the historical data (e.g., `1`).
  - `to` (long, optional): UTC timestamp in milliseconds, set to the current time if not provided.
  - `from` (long, optional): UTC timestamp in milliseconds.
  - `n` (int, optional): The number of bars to be returned, default is 10 and capped at 500.
- **Description:** Retrieves historical bar data for the specified trading pair and interval.

### GetDepthAsync
- **Endpoint:** /api/pro/v1/depth
- **HTTP Method:**  GET
- **Parameters:**
   - `symbol` (string, required): The trading pair symbol (e.g., "ASD/USDT").
- **Description:** Retrieves the order book depth for a specified trading pair.

### GetTradesAsync
- **Endpoint:** /api/pro/v1/trades
- **HTTP Method:** GET
- **Parameters:**
   - `symbol` (string, required): The trading pair symbol (e.g., "ASD/USDT").
   - `n` (int, optional): Number of trades to return, capped at 100.
- **Description:** Retrieves recent trades for a specified trading pair.

## Order Endpoints

### GetOrderAsync
- **Endpoint:** `/api/pro/v1/{accountCategory}/order/status`
- **HTTP Method:** GET
- **Parameters:**
  - `accountGroup` (string, required): The account group.
  - `accountCategory` (string, required): The account category (e.g., "cash").
  - `orderId` (string, required): The order ID.
- **Description:** Retrieves the status of a specific order.
- **Response:** Returns the full response from the AscendEX API.

### GetOpenOrdersAsync
- **Endpoint:** `/api/pro/v1/{accountCategory}/order/open`
- **HTTP Method:** GET
- **Parameters:**
  - `accountGroup` (string, required): The account group.
  - `accountCategory` (string, required): The account category (e.g., "cash").
- **Description:** Retrieves all open orders for a specified account group and category.
- **Response:** Returns the full response from the AscendEX API.

## Orders Endpoint

## Place Order
**Endpoint:** `POST /{accountGroup}/api/pro/v1/{accountCategory}/order`

**Purpose:** Place a new order.

### Required Parameters:
- **symbol**: The trading pair symbol.
- **side**: The side of the order, either "Buy" or "Sell".
- **orderType**: The type of the order, e.g., "Limit", "Market".
- **orderQty**: The quantity of the order.
- **time**: The current timestamp in milliseconds.

### Optional Parameters:
- **id**: Client-provided order ID.
- **orderPrice**: The price for limit orders.
- **stopPrice**: The stop price for stop orders.
- **timeInForce**: Order time in force, e.g., "GTC", "IOC".
- **respInst**: Response instruction, e.g., "ACK", "ACCEPT".

### Example Usage:
```csharp
var orderResult = await client.SpotApi.Trading.PlaceOrderAsync(
    4, "cash", "BTC/USDT", AscendEX.Net.Enums.OrderSide.Buy, AscendEX.Net.Enums.OrderType.Limit, 
    0.001m, "50000", null, null, "GTC", "ACCEPT", default
);
```

---

## Cancel Order
**Endpoint:** `DELETE /{accountGroup}/api/pro/v1/{accountCategory}/order`

**Purpose:** Cancel an existing order.

### Required Parameters:
- **orderId**: The order ID to be canceled.
- **symbol**: The symbol of the order to cancel.
- **time**: The current timestamp in milliseconds.

### Optional Parameters:
- **id**: Client-provided order ID.

### Example Usage:
```csharp
var cancelOrderResult = await client.SpotApi.Trading.CancelOrderAsync(
    4, "cash", "orderId123", "BTC/USDT", null, default
);
```

---

## Cancel All Orders
**Endpoint:** `DELETE /{accountGroup}/api/pro/v1/{accountCategory}/order/all`

**Purpose:** Cancel all open orders.

### Optional Parameters:
- **symbol**: Symbol filter to cancel orders only for a specific symbol.

### Example Usage:
```csharp
var cancelAllOrdersResult = await client.SpotApi.Trading.CancelAllOrdersAsync(
    4, "cash", "BTC/USDT", default
);
```

---

## Get Open Orders
**Endpoint:** `GET /{accountGroup}/api/pro/v1/{accountCategory}/order/open`

**Purpose:** Retrieve open orders.

### Optional Parameters:
- **symbol**: Symbol filter to retrieve open orders only for a specific symbol.

### Example Usage:
```csharp
var openOrdersResult = await client.SpotApi.Trading.GetOpenOrdersAsync(
    4, "cash", "BTC/USDT", default
);
```

---

## Get Order History
**Endpoint:** `GET /api/pro/data/v2/order/hist`

**Purpose:** Retrieve historical orders.

### Required Parameters:
- **account**: Account type: cash/margin/futures, or actual accountId.

### Optional Parameters:
- **symbol**: Symbol filter.
- **startTime**: Start time in milliseconds.
- **endTime**: End time in milliseconds.
- **seqNum**: The sequence number to search from.
- **limit**: Number of records to return (default 500, max 1000).

### Example Usage:
```csharp
var orderHistoryResult = await client.SpotApi.Trading.GetOrderHistoryAsync(
    "cash", "BTC/USDT", startTime: 1622505600000, endTime: 1625097600000, seqNum: 1000, limit: 100, default
);
```


## Usage

Ensure you have the correct API credentials and account group for the endpoints. You can create an instance of `AscendEXRestClient` and use its methods to interact with the API. Here is a basic example of retrieving account information:

```csharp
var client = new AscendEXRestClient(options =>
{
    options.ApiCredentials = new AscendEXApiCredentials("your-api-key", "your-api-secret");
});

var accountInfo = await client.SpotApi.Account.GetAccountInfoAsync();
```

## Websocket

### How to subscribe

```csharp
var accountGroup = "0"; // Replace with actual account group if needed or obtain it with the RestClient.Account.GetAccountInfoAsync()
var logger = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)).CreateLogger<AscendEXSocketClientSpotApi>(); // This is needed to log the messages 
var client = new AscendEXSocketClientSpotApi(logger, accountGroup);

// You can call ConnectToServeAsync with interval or without interval:

var connectResult = await client.ConnectToServerAsync(channel, symbol, data =>
{
      _logger.LogInformation($"Received bar update: {data.Data}");
      Console.WriteLine($"Received bar update: {data.Data}");
}, CancellationToken.None);

// OR

var connectResult = await client.ConnectToServerAsync(channel, interval, symbol, data =>
{
      _logger.LogInformation($"Received bar update: {data.Data}");
      Console.WriteLine($"Received bar update: {data.Data}");
}, CancellationToken.None);

```
### Level 1 Order Book Data (BBO)
For Level 1 Order Book Data you want to use ```ConnectToServerAsync(channel, symbol, data =>{ ... })```

For example:
```csharp
var client = new AscendEXSocketClientSpotApi(logger, accountGroup);
        try
        {
            var symbol = "ASD/USDT";
            var channel = "bbo";

            var connectResult = await client.ConnectToServerAsync(channel, symbol, data =>
            {
                _logger.LogInformation($"Received bar update: {data.Data}");
                Console.WriteLine($"Received bar update: {data.Data}");
            }, CancellationToken.None);

            Assert.NotNull(connectResult);
            Assert.True(connectResult.Success, "Failed to connect to the server for bars.");

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for bars: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
            }
            else
            {
                _logger.LogInformation("Successfully connected to the server for bars.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for bars: {Exception}", ex);
            throw;
        }
```

### Level 2 Order Book Updates
For Level 2 Order Book Updates you want to use ```ConnectToServerAsync(channel, symbol, data =>{ ... })```

For example:
```csharp
var client = new AscendEXSocketClientSpotApi(logger, accountGroup);
        try
        {
            var symbol = "ASD/USDT";
            var channel = "depth";

            var connectResult = await client.ConnectToServerAsync(channel, symbol, data =>
            {
                _logger.LogInformation($"Received bar update: {data.Data}");
                Console.WriteLine($"Received bar update: {data.Data}");
            }, CancellationToken.None);

            Assert.NotNull(connectResult);
            Assert.True(connectResult.Success, "Failed to connect to the server for bars.");

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for bars: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
            }
            else
            {
                _logger.LogInformation("Successfully connected to the server for bars.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for bars: {Exception}", ex);
            throw;
        }
```


### Market Trades 
For Market Trades you want to use ```ConnectToServerAsync(channel, symbol, data =>{ ... })```

For example:
```csharp
var client = new AscendEXSocketClientSpotApi(logger, accountGroup);
        try
        {
            var symbol = "ASD/USDT";
            var channel = "trades";

            var connectResult = await client.ConnectToServerAsync(channel, symbol, data =>
            {
                _logger.LogInformation($"Received bar update: {data.Data}");
                Console.WriteLine($"Received bar update: {data.Data}");
            }, CancellationToken.None);

            Assert.NotNull(connectResult);
            Assert.True(connectResult.Success, "Failed to connect to the server for bars.");

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for bars: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
            }
            else
            {
                _logger.LogInformation("Successfully connected to the server for bars.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for bars: {Exception}", ex);
            throw;
        }
```


### Bar Data 
For Bar Data you want to use ```ConnectToServerAsync(channel, interval, symbol, data =>{ ... })```

For example:
```csharp
        try
        {
            var interval = "1"; // Example interval
            var symbol = "ASD/USDT";
            var channel = "bar";

            var connectResult = await client.ConnectToServerAsync(channel, interval, symbol, data =>
            {
                _logger.LogInformation($"Received bar update: {data.Data}");
                Console.WriteLine($"Received bar update: {data.Data}");
            }, CancellationToken.None);

            Assert.NotNull(connectResult);
            Assert.True(connectResult.Success, "Failed to connect to the server for bars.");

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for bars: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
            }
            else
            {
                _logger.LogInformation("Successfully connected to the server for bars.");
            }

            // Wait for a while to verify the connection and log any received messages
            await Task.Delay(30000);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for bars: {Exception}", ex);
            throw;
        }
```

## TRADING (WebSocket)

### Place Order

Same requirements as RestAPI

Example:

```csharp
        var accountGroup = "4"; // Replace with actual account group if needed
        var accountCategory = "cash"; // Example account category
        var logger = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)).CreateLogger<AscendEXSocketClientSpotApi>();
        var clientOptions = new AscendEXSocketOptions
        {
            ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret)
        };
        var client = new AscendEXSocketClientSpotApi(logger, clientOptions, accountGroup);

        _logger.LogInformation("Starting test for placing an order via WebSocket");

        try
        {
            var symbol = "BTC/USDT";
            var side = OrderSide.Buy;
            var orderType = OrderType.Limit;
            var quantity = 0.00016m;
            var price = "55000";
            var clientOrderId = Guid.NewGuid().ToString();
            var stopPrice = (string?)null;
            var timeInForce = (string?)null;
            var respInst = "ACK";
            var ct = new CancellationToken();

            var result = await client.Trading.PlaceOrderAsync(
                accountCategory,
                symbol,
                side,
                orderType,
                quantity,
                price,
                stopPrice,
                timeInForce,
                respInst,
                ct);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during order placement test: {Exception}", ex);
            throw;
        }
```

### Cancel Order

Same as RestAPI conditions

```csharp
    var result = await tradingClient.CancelOrderAsync("cash", "ETH/USDT", "16e83845dcdsimtrader00008c645f67", ct: CancellationToken.None);
    if (result.Success)
    {
        Console.WriteLine("Order canceled successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to cancel order: {result.Error}");
    }
```

### Cancel all order

Here you can directly cancel all open orders or you can cancel all open orders for specific symbol or symbols

```csharp
    var result1 = await tradingClient.CancelAllOrdersAsync(ct: CancellationToken.None);
    if (result1.Success)
    {
        Console.WriteLine("All orders canceled successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to cancel all orders: {result1.Error}");
    }
```
or

```csharp
    var result2 = await tradingClient.CancelAllOrdersAsync("cash", "BTC/USDT", ct: CancellationToken.None);
    if (result2.Success)
    {
        Console.WriteLine("All orders for BTC/USDT canceled successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to cancel all orders for BTC/USDT: {result2.Error}");
    }
```

### Query Open order

Here you can query a single order by ID or query all open orders, or all open orders in symbol/symbols

Query all open orders
```csharp
var result1 = await tradingClient.QueryOpenOrdersAsync("cash", ct: CancellationToken.None);
if (result1.Success)
{
    Console.WriteLine("Queried all open orders successfully.");
}
else
{
    Console.WriteLine($"Failed to query open orders: {result1.Error}");
}
```

Query open orders for a specific symbol

```csharp
var result2 = await tradingClient.QueryOpenOrdersAsync("cash", "BTC/USDT", ct: CancellationToken.None);
if (result2.Success)
{
    Console.WriteLine("Queried open orders for BTC/USDT successfully.");
}
else
{
    Console.WriteLine($"Failed to query open orders for BTC/USDT: {result2.Error}");
}
```

Query open orders for multiple symbols
```csharp
var result3 = await tradingClient.QueryOpenOrdersAsync("cash", "BTC/USDT,ETH/USDT", ct: CancellationToken.None);
if (result3.Success)
{
    Console.WriteLine("Queried open orders for BTC/USDT and ETH/USDT successfully.");
}
else
{
    Console.WriteLine($"Failed to query open orders for BTC/USDT and ETH/USDT: {result3.Error}");
}
```

## Installation

To use AscendEX.Net, this runs in C# v12. include it in your project by referencing the necessary packages and libraries. Ensure you have the CryptoExchange.Net dependency installed.

For further details, refer to the official AscendEX API documentation.