# Bank Transaction Aggregation API

A high-performance, concurrent REST API built with .NET 8 designed to aggregate customer banking transactions across multiple external financial systems. The application normalizes disparate data schemas into a single, cohesive API contract while supporting real-time filtering, robust pagination, and secure custom API key authentication.

## 🚀 Key Architectural Features

* **Parallel Data Fetching (`Task.WhenAll`):** Minimizes network overhead and latent aggregation delays by querying downstream providers concurrently.
* **Interface-Driven Design (Open-Closed Principle):** New mock banks can be integrated smoothly by simply implementing the `IBankProvider` interface, without altering existing core logic.
* **Global Response Standardization:** Returns data wrapped uniformly inside a consistent JSON object complete with pagination metadata.
* **Custom Security Middleware:** Includes a lightweight Action Filter attribute ensuring protected endpoints are isolated from unauthenticated compute drains.
* **Enterprise Test Coverage:** Engineered using Test-Driven Development (TDD) best practices, backed by an **xUnit** and **Moq** test suite.

---

## 🛠️ Tech Stack & Tooling

* **Backend Framework:** .NET 8 (Web API / C#)
* **Testing Suite:** xUnit, Moq
* **Development Environment:** Ubuntu / Multi-platform .NET CLI

---

## 📁 Directory Structure

```text
├── TransactionAggregator/               # Main API Application
│   ├── Attributes/                      # Custom Security Filters (API Key Auth)
│   ├── Controllers/                     # REST Endpoints
│   ├── Models/                          # Standardized JSON Contract Data Models
│   ├── Services/                        # Business Logic Engine & Bank Abstractions
│   └── Program.cs                       # DI Registrations and App Bootstrapper
│
└── TransactionAggregator.Tests/         # Test Suite
    └── AggregatorServiceTests.cs        # Core business logic unit tests
```
## To Run
1.  - git clone .
    - cd TransactionAggregator/
    - dotnet run 
2. Run the API locally
    - dotnet run --project TransactionAggregator
3. Executing The Test Suite:
    - dotnet test
4. Exemple Request (cURL):
    - curl -i -H "X-API-KEY: PortfolioAggregatorKey2026" \
"http://localhost:5123/api/transactions?category=Groceries&page=1&pageSize=5"