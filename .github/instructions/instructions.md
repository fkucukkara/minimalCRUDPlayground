---
applyTo: '**'
---

# Minimal CRUD Playground - Project Guidelines

## Project Overview
This is a minimal API project built with ASP.NET Core 9.0 that demonstrates CRUD operations for Todo items. The project follows Microsoft's best practices for minimal APIs as outlined in the official documentation.

## Architecture & Design Patterns

### Minimal API Structure
- Use **MapGroup** to organize related endpoints with common URL prefixes
- Implement **TypedResults** instead of Results for better testability and OpenAPI metadata
- Separate endpoint handlers into static methods for better organization and testability
- Use **record types** for immutable data models and DTOs

### Data Layer
- **TodoDBContext**: Entity Framework DbContext for data access
- **Todo**: Main entity model with Id, Name, IsComplete, and Secret properties
- **TodoItemDTO**: Data Transfer Object to prevent over-posting and hide sensitive fields

### Security & Data Protection
- Use DTOs to prevent over-posting vulnerabilities
- Hide sensitive fields (like Secret) from API responses
- Implement proper input validation

## Coding Guidelines

### API Endpoint Design
```csharp
// Use MapGroup for organizing endpoints
var todoItems = app.MapGroup("/todoitems");

// Map to static methods for better testability
todoItems.MapGet("/", GetAllTodos);
todoItems.MapPost("/", CreateTodo);
// etc.
```

### Return Types
- Always use **TypedResults** for better compile-time checking and OpenAPI support
- Return appropriate HTTP status codes:
  - 200 OK for successful GET requests
  - 201 Created for successful POST requests with location header
  - 204 No Content for successful PUT/DELETE requests
  - 404 Not Found when resources don't exist

### Error Handling
- Use proper null checking with pattern matching
- Return appropriate error responses using TypedResults
- Leverage Entity Framework's FindAsync for single entity retrieval

### Database Context Usage
- Inject DbContext into endpoint handlers via dependency injection
- Use async/await for all database operations
- Use appropriate LINQ methods (ToListAsync, ToArrayAsync, etc.)

### Model Conventions
```csharp
// Entity model
public record Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; } // Hidden from API
}

// DTO for API responses
public record TodoItemDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    
    public TodoItemDTO() { }
    public TodoItemDTO(Todo todoItem) =>
        (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
}
```

### Development Environment Setup
- Use in-memory database for development/testing
- Enable OpenAPI/Swagger in development environment
- Configure HTTPS redirection
- Use nullable reference types for better null safety

### Dependencies
- **Microsoft.EntityFrameworkCore.InMemory**: For in-memory database provider
- **Microsoft.AspNetCore.OpenApi**: For OpenAPI/Swagger documentation

## API Endpoints Structure
| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| GET | /todoitems | Get all todo items | None | Array of TodoItemDTO |
| GET | /todoitems/complete | Get completed todo items | None | Array of TodoItemDTO |
| GET | /todoitems/{id} | Get todo item by ID | None | TodoItemDTO or 404 |
| POST | /todoitems | Create new todo item | TodoItemDTO | Created TodoItemDTO |
| PUT | /todoitems/{id} | Update existing todo item | TodoItemDTO | 204 No Content |
| DELETE | /todoitems/{id} | Delete todo item | None | 204 No Content |

## Best Practices to Follow

1. **Dependency Injection**: Use constructor injection for services and DbContext
2. **Async Programming**: Use async/await for all I/O operations
3. **Input Validation**: Validate inputs and handle edge cases appropriately
4. **Resource Management**: Let DI container handle DbContext lifecycle
5. **Testing**: Structure code to be easily testable with static methods
6. **Documentation**: Leverage OpenAPI for API documentation
7. **Security**: Use DTOs to prevent over-posting and data exposure

## When Making Changes
- Maintain the existing minimal API structure
- Use TypedResults for all endpoint responses  
- Follow the established naming conventions
- Ensure DTOs are used for all external API contracts
- Add appropriate error handling for new endpoints
- Update OpenAPI documentation if adding new endpoints