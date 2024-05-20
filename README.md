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

## Usage

Ensure you have the correct API credentials and account group for the endpoints. You can create an instance of `AscendEXRestClient` and use its methods to interact with the API. Here is a basic example of retrieving account information:

```csharp
var client = new AscendEXRestClient(options =>
{
    options.ApiCredentials = new AscendEXApiCredentials("your-api-key", "your-api-secret");
});

var accountInfo = await client.SpotApi.Account.GetAccountInfoAsync();
```
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


## Installation

To use AscendEX.Net, include it in your project by referencing the necessary packages and libraries. Ensure you have the CryptoExchange.Net dependency installed.

For further details, refer to the official AscendEX API documentation.