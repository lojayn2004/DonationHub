# DonationHub

DonationHub is an ASP.NET Core Web API for managing donations, campaigns, categories, payments, and users. The solution follows a layered architecture and uses Entity Framework Core with SQL Server.

## Prerequisites

- .NET 10 SDK
- SQL Server 
- Entity Framework Core CLI tools:

```powershell
dotnet tool install --global dotnet-ef
```

Verify the tools are installed:

```powershell
dotnet --version
dotnet ef --version
```

## Configuration

The API reads configuration from `DonationHub.API/appsettings.json` and, in local development, from ASP.NET Core user secrets.

[appsettings-example.json](DonationHub.API/appsettings-example.json) shows the non-secret configuration shape. Copy it to `DonationHub.API/appsettings.json` when setting up the project, and update it with your SQL Server connection string and local JWT issuer/audience values. `appsettings.json` is ignored by Git; never commit real credentials, connection strings, JWT keys, or passwords.

Typical development connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DonationHub;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Adjust the server name for your SQL Server installation.

### Local user secrets

From the repository root, initialize user secrets for the API project:

```powershell
dotnet user-secrets init --project DonationHub.API
```

Set a JWT signing key of at least 32 characters. ASP.NET Core combines `JwtOptions` with the secret-key name `SecretKey`:

```powershell
dotnet user-secrets set "JwtOptions:SecretKey" "<generate-a-private-at-least-32-character-key>" --project DonationHub.API
```

Set the seeded administrator password under the option section used by the application:

```powershell
dotnet user-secrets set "AdminSeedOptions:Password" "<strong-local-admin-password>" --project DonationHub.API
```

Configure the administrator email and display name in `DonationHub.API/appsettings.json`:

```json
"AdminSeedOptions": {
  "Email": "<local-admin-email>",
  "FullName": "<local-admin-name>"
}
```

Keep the non-secret values in `appsettings.json`. Use user secrets only for sensitive values, such as the administrator password and JWT signing key.

These commands store values in the current operating-system user secret store, not in the repository. User secrets override matching keys from `appsettings.json`. The project is registered with:

```powershell
dotnet user-secrets list --project DonationHub.API
```

### Development users

Development startup seeding currently creates these accounts when the users table is empty:

| Role | Email | Development password |
|---|---|---|
| Admin | `Admin@gmail.com` | The value configured by `AdminSeedOptions:Password` |
| Donor | `donor@example.com` | `Donor@1234` |
| Campaign Manager | `campaignmanager@example.com` | `CampaignManager@1234` |



## Database migrations

Run migrations from the repository root. Because migrations and the database context are in the infrastructure project, use the API project as the startup project:

```powershell
dotnet ef database update --project DonationHub.Infrastructure --startup-project DonationHub.API
```

This applies all pending migrations to the database configured by `DefaultConnection`.

Create a new migration after changing the EF Core model:

```powershell
dotnet ef migrations add <MigrationName> --project DonationHub.Infrastructure --startup-project DonationHub.API
```

Remove the latest unapplied migration during local development:

```powershell
dotnet ef migrations remove --project DonationHub.Infrastructure --startup-project DonationHub.API
```

## Run the API

Restore, build, and run the API:

```powershell
dotnet restore DonationHub.slnx
dotnet run --project DonationHub.API
```

The HTTPS profile uses `https://localhost:7282` and `http://localhost:5220`; the HTTP profile uses `http://localhost:5220`. The exact URL shown by the running application is authoritative.

Startup in `Development` exposes the API schema and runs seeders. The database must already exist or be migrated before seeding succeeds.

## Scalar API reference

With the API running in the `Development` environment, open:

- `https://localhost:7282/scalar/v1`
- or the scalar URL under the actual HTTP/HTTPS URL printed by the API

Scalar is the API documentation UI used by this project. The OpenAPI document is served by ASP.NET Core at `/openapi/v1.json`.

This project uses Scalar instead of Swagger UI.

## Test authentication with Scalar

1. Open Scalar.
2. Call `POST /api/v1/auth/login`.
3. Enter the email and password of a seeded account.
4. Copy the access token from the response.
5. Configure Scalar authorization with:
   - Scheme: `Bearer`
   - Token: the access token only, without the word `Bearer`
6. Call a protected endpoint, such as:
   - `GET /api/v1/auth/me`
   - `POST /api/admin/staff` (requires the Admin role)

Example login body:

```json
{
  "email": "donor@example.com",
  "password": "Donor@1234"
}
```

The response contains the access token used for secured requests. The refresh token is handled through the configured HTTP-only cookie.

Admin-only endpoint checks require a token returned after logging in with the seeded admin account. Log out through `POST /api/v1/auth/logout` while the refresh-token cookie is available.

## Campaign endpoints

Campaign routes use the singular `/api/v1/campaign` path.

| Method | Route | Access | Description |
|---|---|---|---|
| `GET` | `/api/v1/campaign` | Public | List active campaigns |
| `GET` | `/api/v1/campaign/{id}` | Public | Get a campaign by ID |
| `GET` | `/api/v1/campaign/success-stories` | Public | List success-story campaigns |
| `POST` | `/api/v1/campaign` | Admin | Create a campaign |
| `PUT` | `/api/v1/campaign/{id}` | Admin | Update campaign details |
| `PATCH` | `/api/v1/campaign/{id}` | Admin | Update campaign status |
| `DELETE` | `/api/v1/campaign/{id}` | Admin | Delete a campaign |

For admin routes, send `Authorization: Bearer <access-token>`. Read responses choose Arabic fields when the request's `Accept-Language` header starts with `ar` (for example, `ar` or `ar-SA`); otherwise they return English fields. For example:

```http
GET /api/v1/campaign
Accept-Language: ar
```

The request/response schemas and validation details are available in the OpenAPI document and Scalar API reference.

## Run tests

Run all test projects from the repository root:

```powershell
dotnet test .\DonationHub.slnx
```

To run a specific test project, for example the application unit tests:

```powershell
dotnet test .\DonationHub.Application.UnitTests\DonationHub.Application.UnitTests.csproj
```

Integration tests run the API against SQL Server. They use the connection string in the `TEST_DB_CONNECTION` environment variable; if it is not set, they use `Server=.;Database=DonationHubTestDb_v2;Trusted_Connection=True;TrustServerCertificate=True`. The test host calls `EnsureCreated`, which creates a missing database but does not clear an existing one. Use a new, empty database for each integration-test run so prior test data cannot affect results. For example, in PowerShell:


This creates a uniquely named test database for that run, provided the SQL Server instance is available and the account can create databases. Do not point `TEST_DB_CONNECTION` at a development or production database. The test database remains on the server after the tests finish; remove it using your SQL Server administration tools when it is no longer needed.

## Solution structure

- `DonationHub.API` - HTTP endpoints, API configuration, authentication, and Scalar/OpenAPI setup
- `DonationHub.Application` - use cases, commands, queries, validation, and application contracts
- `DonationHub.Domain` - domain entities, rules, exceptions, and domain concepts
- `DonationHub.Infrastructure` - EF Core persistence, SQL Server configuration, migrations, Identity, JWT services, and seeders
- `DonationHub.Shared` - options classes and configuration contracts shared across projects

## Sprint architecture choices explanation


1. **Layered architecture** 
   - separates API concerns from application behavior, domain rules, persistence, and shared configuration. 
   - Alternative: a simpler single-project app.
   -  Trade-off: more projects and references, but stronger boundaries and maintainability.
2. **Mediator/CQRS-style application features**
   - keeps commands and queries focused and testable. 
   - Alternative: controllers calling services directly.
   - Trade-off: additional abstractions and registration, but clearer use-case boundaries.

3. **Repository and unit-of-work abstractions**
 - centralize persistence operations and coordinate writes.
 - Alternative: injecting `DbContext` directly everywhere.
 - Trade-off: extra indirection, but consistency and easier mocking/replacement.
5. **Soft-delete interceptor for categories** 
 - preserves records and supports audit/recovery-oriented behavior. 
 - Alternative: physical deletion. 
 - Trade-off: Easier and more generalized than deleting every entity separatly

6. **User secrets for local credentials** 
  - keeps local keys and passwords out of source control.
  - Alternative: environment variables or committed configuration. 
  - Trade-off: setup is developer-machine-specific, but secrets are isolated from Git.

7. **Options classes with validation**
 - fail startup when required configuration is missing or invalid. 
 - Alternative: reading configuration directly with string keys. 
 - Trade-off: slightly more setup, but configuration errors surface early and have types.

8. **HTTP-only refresh-token cookie** 
- limits JavaScript access to the refresh token.
- Alternative: browser storage. 
- Trade-off: cookie and cross-site configuration require careful handling, but script-based token theft is reduced.
9. **Domain exceptions and Result objects** 
- communicate expected failures without relying only on exceptions for control flow. 
- Alternative: exceptions or raw service responses.
- Trade-off: more mapping and result handling, but clearer application outcomes and API problem responses.
10. **Separate query contract (`IQuery`)**
 - keeps read operations distinct from write-oriented repository operations. The query implementation uses no-tracking reads and specification projections, reducing unnecessary entity tracking and allowing read use cases to shape the data they need.
 - Alternative: use the same persistence abstraction for both reads and writes.
 - Trade-off: one additional contract, in exchange for clearer read/write responsibilities and a focused place to optimize read queries.
11. **Specification pattern**
 - packages query criteria, includes, and projections into reusable, testable definitions, keeping persistence query details out of individual handlers.
 - Alternative: write query expressions directly in each handler.
 - Trade-off: some extra types, but shared query rules are easier to reuse and maintain.
12. **Concurrency contract (`IConcurrencyHandler<TEntity>`)**
 - sets the entity's original row-version value from the version supplied by the caller. When the entity is saved, EF Core can detect that another request changed the row and the application can report the conflict rather than silently overwriting newer data.
 - Alternative: omit row-version checks or implement EF Core concurrency handling separately in each write handler.
 - Trade-off: callers must provide the version they read, but concurrent updates are detected consistently.
13. **Temporary header-based language selection**
 - reads `Accept-Language` and selects Arabic fields for `ar`/`ar-*`; all other values currently use English. This is a lightweight placeholder, not full localization. A proper localization solution is planned for Sprint 2.
