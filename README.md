Unit Conversion API

This project is an ASP.NET Core Web API that converts numerical values between units for length, temperature, and weight/mass. The implementation is intentionally simple: units and conversion logic are hardcoded for the exercise and intended to be easy to extend.

What I added for you
- Conversion endpoints (GET + POST) at `api/conversion` implemented in `Controllers/ConversionController.cs`
- `UnitConversionService` in `Services/UnitConversionService.cs` implementing length, weight, and temperature conversions
- `IUnitConversionService` interface and `ConversionResult` record in `Services/IUnitConversionService.cs`
- Basic unit tests in the `UnitConversion.Tests` project
- OpenAPI (Swagger) is enabled and mounted for easy testing

How to run locally

Requirements
- .NET 10 SDK installed
- (Optional) Visual Studio 2026 or VS Code

Run the API
1. From the repository root run:
   `dotnet run --project UnitConversion.API`
2. The app will start and expose Swagger UI at `https://localhost:5086/swagger` (port shown in console). Use Swagger or the examples below.

Run tests
1. From the repository root run:
   `dotnet test UnitConversion.Tests`

API usage examples

GET example (query params)
- Request:
  `GET https://localhost:5001/api/conversion?from=meters&to=feet&value=1`
- Response (200 OK)
  ```json
  {
    "from": "meters",
    "to": "feet",
    "input": 1.0,
    "output": 3.28084,
    "category": "length"
  }
  ```

POST example (JSON body)
- Request:
  `POST https://localhost:5001/api/conversion`
  Body:
  ```json
  { "from": "celsius", "to": "fahrenheit", "value": 100 }
  ```
- Response (200 OK)
  ```json
  {
    "from": "celsius",
    "to": "fahrenheit",
    "input": 100.0,
    "output": 212.0,
    "category": "temperature"
  }
  ```

Supported units (non-exhaustive)
- Length: meter (m), kilometer (km), centimeter (cm), millimeter (mm), foot (ft), inch (in), mile (mi)
- Weight: kilogram (kg), gram (g), milligram (mg), pound (lb), ounce (oz)
- Temperature: celsius (c), fahrenheit (f), kelvin (k)

Design notes and trade-offs
- Units are hardcoded in `UnitConversionService` for simplicity and speed of delivery. For a production system you would store units and relationships in a data store or configuration (JSON/YAML/DB) and add admin APIs for managing them.
- Linear conversions use a base unit and multiplicative factors. Temperature uses explicit formulas.
- The service is small and registered as a singleton.
- No authentication, rate limiting, or persistence is provided in this iteration.

Extending the project
- Move unit definitions to a JSON file and load them at startup.
- Add admin endpoints to create/update/remove units.
- Add caching, logging, metrics, and CI/CD.
