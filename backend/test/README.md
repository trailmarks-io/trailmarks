# Trailmarks Backend Tests

This directory contains comprehensive unit tests for the Trailmarks backend API.

## Test Framework

- **xUnit** - Primary test framework
- **Moq** - Mocking library for dependencies
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database for testing
- **Microsoft.AspNetCore.Mvc.Testing** - Testing utilities for ASP.NET Core

## Project Structure

```
backend/test/
├── Controllers/          # Controller tests
│   ├── HealthControllerTests.cs
│   ├── WandersteineControllerTests.cs
│   └── TranslationsControllerTests.cs
├── Services/            # Service tests
│   └── DatabaseServiceTests.cs
├── Models/              # Model tests
│   └── WandersteinResponseTests.cs
└── TrailmarksApi.Tests.csproj
```

## Running Tests

From the `backend/test` directory:

```bash
dotnet test
```

With detailed output:

```bash
dotnet test --verbosity normal
```

## Test Coverage

### Controllers (18 tests)

#### HealthController (3 tests)
- Returns OK result
- Returns healthy status
- Returns correct service name

#### WandersteineController (6 tests)
- Returns OK result for recent items
- Returns maximum 5 items
- Orders items by CreatedAt descending
- Returns all items
- Returns empty list when no data

#### TranslationsController (9 tests)
- Returns OK result with translations
- Returns NotFound for invalid language
- Builds nested dictionary structure
- Case-insensitive language matching
- Returns distinct supported languages
- Returns empty list when no translations
- Returns languages in alphabetical order

### Services (5 tests)

#### DatabaseService (5 tests)
- Initializes successfully
- Seeds Wandersteine data
- Seeds translation data
- Seeds multiple languages
- Prevents duplicate data on multiple calls

### Models (3 tests)

#### WandersteinResponse (4 tests)
- Maps all properties correctly
- Formats CreatedAt in ISO 8601
- Handles empty strings
- Preserves ID values

## Test Patterns

### TestContext Base Class

Tests that require database access inherit from `TestContext`, which provides a shared `GetInMemoryContext()` method:

```csharp
public class MyControllerTests : TestContext
{
    [Fact]
    public async Task MyTest()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new MyService(context);
        
        // Act & Assert
        // ...
    }
}
```

This eliminates code duplication across test classes.

### Arrange-Act-Assert

All tests follow the AAA pattern:

```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var context = GetInMemoryContext();
    var service = new MyService(context);
    
    // Act
    var result = await service.DoSomething();
    
    // Assert
    Assert.NotNull(result);
}
```

### In-Memory Database

The `TestContext` base class provides access to Entity Framework's in-memory database provider. Each test gets a fresh database instance with a unique name to ensure test isolation.

### Mocking with Moq

Dependencies are mocked using Moq:

```csharp
var logger = new Mock<ILogger<MyController>>();
var controller = new MyController(context, logger.Object);
```

## Writing New Tests

When adding new functionality to the backend:

1. Create a corresponding test file in the appropriate directory
2. Follow the existing naming convention: `{ClassName}Tests.cs`
3. Use xUnit `[Fact]` attribute for test methods
4. Use descriptive test method names: `MethodName_Scenario_ExpectedBehavior`
5. Include both success and error scenarios
6. Mock external dependencies
7. Clean up resources in tests (use `using` statements or proper disposal)

## Continuous Integration

These tests are run automatically in the CI/CD pipeline to ensure code quality and prevent regressions.
