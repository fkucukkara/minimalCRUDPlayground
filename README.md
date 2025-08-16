# minimalCRUDPlayground — Minimal ASP.NET Core CRUD example

This repository is a small, educative ASP.NET Core minimal-API that implements a Todo list with CRUD operations using Entity Framework Core's InMemory provider and OpenAPI support.

This README explains what the project contains, how the code is organized, how to run it locally, and how the endpoints work. It's intentionally short and focused so you can use the project to learn the important pieces quickly.

## Checklist (requirements extracted from your request)
- Create an educative `README.md` for the repo — Done
- Explain code layout and important files (`Program.cs`, `TodoDBContext.cs`, models) — Done
- Document how to run, test and query the API (Windows/pwsh) — Done
- Show example requests and explain DTO/security considerations — Done

## Quick summary
- Framework: .NET 9 (see `minimalCRUDPlayground.csproj`)
- Pattern: Minimal APIs (single-file `Program.cs`) with EF Core InMemory database
- OpenAPI is enabled in development to expose interactive docs

## What is in this repo (important files)
- `minimalCRUDPlayground/Program.cs` — The minimal API. Registers services, maps endpoints (RouteGroup `/todoitems`) and contains the handler logic.
- `minimalCRUDPlayground/TodoDBContext.cs` — EF Core `DbContext` for `Todo` entities.
- `minimalCRUDPlayground/minimalCRUDPlayground.csproj` — Project file; references `Microsoft.AspNetCore.OpenApi` and `Microsoft.EntityFrameworkCore.InMemory`.
- `minimalCRUDPlayground/minimalCRUDPlayground.http` — A collection of example HTTP requests you can run from an editor or copy into a tool like curl or Postman.
- `minimalCRUDPlayground/appsettings.json` and `appsettings.Development.json` — Configuration files for different environments.
- `global.json` — .NET SDK version configuration (targets .NET 9.0).

## Project Structure
```
minimalCRUDPlayground/
├── minimalCRUDPlayground/
│   ├── Program.cs              # Main application entry point with minimal API endpoints
│   ├── TodoDBContext.cs        # Entity Framework DbContext
│   ├── minimalCRUDPlayground.csproj  # Project file with dependencies
│   ├── minimalCRUDPlayground.http    # HTTP request examples
│   ├── appsettings.json        # Application configuration
│   ├── appsettings.Development.json  # Development environment config
│   └── Properties/
│       └── launchSettings.json # Launch profiles for development
├── README.md                   # This file
├── LICENSE                     # MIT License
├── global.json                 # .NET SDK version configuration
└── minimalCRUDPlayground.sln   # Visual Studio solution file
```

## Dependencies
This project uses the following key NuGet packages:
- **Microsoft.AspNetCore.OpenApi** (9.0.8) — Provides OpenAPI/Swagger documentation support
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.8) — In-memory database provider for Entity Framework Core

Target Framework: **.NET 9.0** (as specified in global.json)

## Development Environment
The project is configured with:
- **Visual Studio 2022** support (solution file included)
- **HTTPS development certificates** for secure local development
- **Hot reload** capabilities for rapid development
- **Nullable reference types** enabled for better null safety

## Code contract (small)
- Inputs/Outputs:
  - POST /todoitems/ accepts a JSON `TodoItemDTO` and returns 201 Created with the saved DTO.
  - GET endpoints return DTOs (not the full entity with secrets).
- Data shapes:
  - `Todo` (entity): { Id: int, Name: string?, IsComplete: bool, Secret: string? }
  - `TodoItemDTO` (DTO): { Id: int, Name: string?, IsComplete: bool }
- Error modes: NotFound (404) when requesting or updating a non-existent ID; returns NoContent (204) after successful delete/update where appropriate.

## Why a DTO?
The `Todo` entity contains a `Secret` field which must not be exposed via API responses. Handlers map `Todo` to `TodoItemDTO` and return the DTO, demonstrating a simple data privacy pattern.

## Endpoints
All endpoints are grouped under `/todoitems` via a `RouteGroupBuilder` in `Program.cs`:

- GET /todoitems/ — returns all todos (as DTOs)
- GET /todoitems/complete — returns only completed todos
- GET /todoitems/{id} — returns a single todo by id
- POST /todoitems/ — creates a new todo (provide `name` and `isComplete` in JSON)
- PUT /todoitems/{id} — updates an existing todo (sends 404 if not found)
- DELETE /todoitems/{id} — deletes a todo (sends 404 if not found)

Example JSON for create/update:
{
  "name": "Learn ASP.NET Core",
  "isComplete": false
}

The `minimalCRUDPlayground/minimalCRUDPlayground.http` file contains ready-to-use examples that match these routes.

## How to run (Windows PowerShell — quick start)
Prerequisite: install .NET SDK 9.x (the project targets `net9.0`).

Open PowerShell in the repo root and run:

```powershell
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project .\minimalCRUDPlayground\minimalCRUDPlayground.csproj

# Build for release
dotnet build --configuration Release
```

When running in development, OpenAPI (Swagger) is mapped automatically; check the console output for the launched URL (e.g., https://localhost:5001 or http://localhost:5000 depending on your config).

You can query the API with curl, HTTPie, Postman, or the included `.http` file. Example using curl:

```bash
# create a todo
curl -X POST http://localhost:5003/todoitems/ -H "Content-Type: application/json" -d '{"name":"Test","isComplete":false}'

# get all
curl http://localhost:5003/todoitems/
```

(adjust the port to match the URL printed when the app starts).

## How the code works (walkthrough)
- The app registers services:
  - `builder.Services.AddOpenApi()` — adds OpenAPI/Swagger support used in development.
  - `builder.Services.AddDbContext<TodoDBContext>(opt => opt.UseInMemoryDatabase("TodoList"));` — registers an EF Core InMemory DB.
- `RouteGroupBuilder todoItems = app.MapGroup("/todoitems");` then maps handlers for each HTTP verb.
- Handlers use `TodoDBContext` injected by DI to query and mutate data.
- Mapping from `Todo` to `TodoItemDTO` is done inline (there's a small record constructor that accepts a `Todo`).

Important implementation choices you can learn from:
- Minimal API style keeps everything compact and is great for demos and small services.
- Use DTOs to control what fields leave your application.
- Use `TypedResults` to return strongly-typed HTTP results (Ok, Created, NotFound, NoContent).

## Extending this project
- Replace InMemory DB with a persistent provider (e.g., SQL Server or Postgres): update `AddDbContext` and add a connection string in `appsettings.json`.
- Add validation to DTOs (e.g., data annotations) and return appropriate 400 responses.
- Add automated tests: create xUnit tests that spin up the WebApplicationFactory for integration testing.

## Contributing
This project follows clean code principles and minimal API best practices. When contributing:
- Keep the minimal API pattern intact
- Maintain the DTO security pattern (never expose internal entity fields)
- Follow the existing code style and naming conventions
- Ensure all endpoints return appropriate HTTP status codes

## References

- Microsoft tutorial: "Create a minimal web API in ASP.NET Core" — https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0&tabs=visual-studio

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.