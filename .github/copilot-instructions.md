# MyAPI_1 Codebase Guide for AI Agents

## Project Overview
MyAPI_1 is a minimal ASP.NET 8.0 Web API using Entity Framework Core with MySQL. The architecture emphasizes simplicity with minimal APIs (no traditional controllers), all endpoint definitions live in `Program.cs`.

## Architecture & Key Components

### Stack
- **Framework**: ASP.NET Core 8.0 (Nullable enabled, Implicit usings)
- **Database**: MySQL via `Pomelo.EntityFrameworkCore.MySql` v9.0.0
- **Documentation**: Swagger/OpenAPI via Swashbuckle
- **Port**: `http://localhost:5085` (HTTP), `https://localhost:7216` (HTTPS)

### Project Structure
```
Data/
  └─ AppDbContext.cs       # DbContext - declare DbSet properties for entities
Models/
  └─ UserProfile.cs        # Entity models with public properties (no [Key] needed for Id)
Program.cs                 # ALL endpoint definitions and DI configuration
```

## Core Patterns

### Minimal API Pattern
All endpoints are defined in `Program.cs` using `app.MapGet()`, `app.MapPost()`, etc. No controller classes.

**Example Pattern**:
```csharp
app.MapGet("/api/profile", async (AppDbContext db) => 
    await db.Profiles.ToListAsync());
```
- Dependencies (like `AppDbContext`) are injected via method parameters
- Use `async/await` for database operations
- Return `IAsyncEnumerable<T>` or `Task<T>` for automatic JSON serialization

### Entity Models
Located in `Models/` folder. Properties use auto-properties with default empty string initialization:
```csharp
public class UserProfile
{
    public int Id { get; set; }  // EF Core auto-detects as primary key
    public string FieldName { get; set; } = string.Empty;
}
```

### Database Configuration
Declared in `Data/AppDbContext.cs`:
```csharp
public DbSet<UserProfile> Profiles { get; set; }  // Creates "Profiles" table
```

Connection string from `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=myweb;user=root;password="
}
```

## Development Workflow

### Running the API
```bash
dotnet run                           # Starts on port 5085, opens Swagger UI
dotnet run --launch-profile https    # Runs HTTPS profile (port 7216)
```

### Testing Endpoints
Use `.http` files (e.g., `MyAPI_1.http`) with REST Client:
```
@MyAPI_1_HostAddress = http://localhost:5085

GET {{MyAPI_1_HostAddress}}/api/profile
Accept: application/json
```

### Database
- MySQL must be running on `localhost:3306`
- Database: `myweb`, user: `root`
- EF Core handles table creation via model-first approach

## When Adding New Features

### Add a New Entity
1. Create class in `Models/` with `Id` property and auto-properties
2. Add `public DbSet<EntityName> EntityNames { get; set; }` to `AppDbContext`
3. Migrations: Not currently present—table created from model definition

### Add a New Endpoint
1. Define in `Program.cs` using `app.MapGet()`, `app.MapPost()`, etc.
2. Inject `AppDbContext db` as parameter for database operations
3. Use `async/await` for DB calls: `await db.EntityName.ToListAsync()`
4. Update `.http` file to test new endpoint

### Configuration
- App settings: Modify `appsettings.json` (production) or `appsettings.Development.json` (dev)
- Logging: Configured in appsettings with default level "Information"

## Common Patterns & Conventions

- **Async-First**: All DB operations use `ToListAsync()`, `SaveChangesAsync()`, etc.
- **Dependency Injection**: DbContext and other services injected via method parameters in minimal APIs
- **Naming**: PascalCase for classes/properties, snake_case in URLs (`/api/profile`)
- **Return Types**: Minimal APIs auto-serialize to JSON; return objects directly or async Task<T>
- **Configuration**: Use `builder.Configuration.GetConnectionString()` for secrets/connection strings

## Files to Reference
- [Program.cs](../Program.cs) - Entry point, DI setup, all endpoints
- [Data/AppDbContext.cs](../Data/AppDbContext.cs) - Database context, entity sets
- [Models/UserProfile.cs](../Models/UserProfile.cs) - Example entity model
- [appsettings.json](../appsettings.json) - Configuration, connection strings
