# ApiAggregations

**ApiAggregations** is a .NET 10 Web API project that integrates multiple external APIs and exposes aggregated data through clean, maintainable endpoints.

## Features

- Integrates with external APIs (e.g., JSONPlaceholder)
- Built using Clean Architecture principles
- Fully async API calls with proper exception handling
- Unit and Integration tests included
- Dependency Injection for all services
- Swagger/OpenAPI documentation for easy exploration

## Architecture
Clean Architecture + Modular API Clients

## Project Structure

ApiAggregations/
│
├─ Api/ # API project, contains controllers
├─ Application/ # Business logic, services, DTOs
├─ Domain/ # Core entities and models
├─ Infrastructure/ # API clients, repositories, persistence
├─ Shared/ # Shared utilities and constants
└─ Tests/ # Unit & integration tests


## Getting Started

Ρόλος κάθε Layer

1. API Layer (Presentation)

ASP.NET Core Controllers

Ρόλος:

Receives HTTP requests
Validates input
Calls Application Services
Returns DTO responses
No business logic.

2. Application Layer

Core Orchestration Logic

Ρόλος:

Orchestrates multiple API calls
Aggregates data
Applies filtering & sorting
Coordinates caching & fallback

Patterns:

Facade
Strategy
Pipeline

3. Domain Layer

Pure Business Logic

Περιέχει:

Aggregated models
Performance bucket logic
Rules για relevance, sorting κλπ

❗ No frameworks depedencies.

4. Infrastructure Layer

External integrations

Περιέχει:

HTTP API clients
Cache providers
Stats store
Polly retry policies
Background workers

Patterns:

Adapter
Decorator
Repository 
Circuit Breaker

5. Shared / Cross-Cutting

Logging
Exception handling
Middleware
Common utilities

Design Patterns

Strategy Pattern — για διαφορετικά APIs
Κάθε API = strategy

Facade Pattern — για Aggregation
Κρύβει πολυπλοκότητα από controllers

Adapter Pattern — για μετατροπή API responses → internal models
Κάθε API έχει διαφορετικό schema

Polly (Retry, Timeout, Circuit Breaker)
Resilience layer

Decorator — για caching / logging API calls
Διαχωρίζει concerns

Repository Pattern — για Statistics Store
(In-Memory also)

Observer / Background Worker — για anomaly detection


### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10)
- Optional: Docker (if using Dockerfile for containerization)

### Running the API

```bash
# Clone the repository
git clone https://github.com/vvarvi/ApiAggregations.git
cd ApiAggregations

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the API
dotnet run --project ApiAggregations.Api

The API will start on https://localhost:5194

Example API Calls
curl https://localhost:5001/api/aggregation

http://localhost:5194/api/auth/login?userId=demo&role=User

Swagger Documentation
http://localhost:5194/swagger/index.html

Testing

# Run all unit and integration tests
dotnet test

Unit tests use mocks to isolate business logic
Integration tests verify full end-to-end API behavior

Logging & Error Handling

All API requests are logged using ILogger<T>
Global exception handling middleware ensures consistent error responses
Proper use of async/await for all external calls

