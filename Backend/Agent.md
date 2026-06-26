# Brain

## Project Overview

Enterprise Backend Application

Technology Stack

- ASP.NET Core (.NET 10)
- Entity Framework Core
- SQL Server
- REST API
- JWT Authentication
- FluentValidation
- AutoMapper
- Dependency Injection
- Localization
- Clean Architecture

---

# Architecture

API
↓
Application Services
↓
Business Services
↓
Repository / Data Access
↓
Entity Framework Core
↓
SQL Server

---

# Current Focus

Backend Development Only.

Ignore any frontend or Angular-related suggestions unless I explicitly ask.

Do not generate Angular code.

Do not suggest frontend architecture.

---

# Coding Standards

- Always use async/await.
- Always use Dependency Injection.
- Always create interfaces.
- Never duplicate business logic.
- Reuse existing services whenever possible.
- Keep methods small and maintainable.
- Follow SOLID principles.
- Use constructor injection only (Parameterized Constructor). (Most common and widely understood.)
- Never use static helper classes unless already used.

---

# Service Rules

- Follow the existing BaseService pattern.
- Use ServiceContext where applicable.
- Reuse existing infrastructure.
- Do not introduce new architectural patterns without request.
- Prefer extending existing services over creating new ones.

---

# Database Rules

- SQL Server
- Entity Framework Core
- Stored Procedure First (where project already follows this)
- Prefer IQueryable until final execution.
- Use AsNoTracking() for read-only operations.
- Avoid unnecessary ToList().
- Avoid N+1 queries.
- Use projections whenever possible.
- Optimize LINQ before writing SQL.

---

# API Rules

- RESTful APIs
- Proper HTTP Status Codes
- Validation using FluentValidation
- Global Exception Middleware
- Result<T> response pattern
- PagedResultDto<T> for listing APIs

---

# Authentication

- JWT Authentication

---

# Exception Handling

Use existing exception classes.

Examples:

- ValidationException
- NotFoundException
- UnauthorizedException
- ForbiddenException
- AppException

Do not create duplicate exception handling.

---

# Logging

Use ILogger.

Reuse existing logging strategy.

---

# Performance Rules

Always think about performance before generating code.

Prefer

- IQueryable
- Async
- Bulk Operations
- Projection
- AsNoTracking()

Avoid

- Reflection
- Multiple DB Calls
- Duplicate Queries
- Nested Loops
- Unnecessary Memory Allocation

---

# Response Rules

Before generating code:

1. Understand existing implementation.
2. Explain the approach.
3. Explain impact.
4. Then generate production-ready code.

Never generate demo code.

Never generate pseudo code.

Never guess project structure.

If required information is missing, ask which file is needed.

---

# AI Instructions

Always read this file first.

Treat this file as the project source of truth.

Do not scan the whole repository unless explicitly requested.

Follow the existing project architecture.

Extend existing code instead of redesigning it.

If multiple solutions exist, recommend the one that best fits the current architecture.

Optimize for maintainability, readability, performance, and enterprise scalability.

---

## 📂 Project Directory Structure

```
Backend/
├── Backend.slnx                            # VS Solution File
├── dotnet-tools.json                       # Dotnet tools configuration
│
├── Domain/                                 # Enterprise Core & Entities (No dependencies)
│   ├── Common/
│   │   ├── AuditableEntity.cs              # Base class for tracking entity changes
│   │   └── BaseEntity.cs                   # Base class for database entity primary keys
│   ├── Entities/
│   │   ├── Employees/
│   │   │   └── Employee.cs                 # Employee entity
│   │   ├── JobTitles/
│   │   │   └── JobTitle.cs                 # JobTitle entity
│   │   ├── Users/
│   │   │   └── User.cs                     # User entity
│   │   └── UserSessions/                   # Directory for user session entities
│   ├── Enums/
│   │   └── LoginProvider.cs                # Enums for providers (e.g., local logins)
│   └── Domain.csproj
│
├── Application/                            # Core Business Logic & Use Cases (Only depends on Domain)
│   ├── Common/
│   │   ├── Contexts/
│   │   │   └── UserContext.cs              # Application-specific User Context
│   │   ├── Extensions/
│   │   │   ├── ApplicationServiceCollection.cs  # Injection registration of services
│   │   │   ├── ValidationServiceCollection.cs   # FluentValidation registration
│   │   │   └── ValidatorExtensions.cs           # FluentValidation custom validator rules
│   │   ├── Helpers/
│   │   │   └── IPasswordHelper.cs          # Hashing interface definition
│   │   ├── Interfaces/
│   │   │   ├── Auth/
│   │   │   │   └── ICurrentUserContext.cs  # Interface to retrieve active user identity
│   │   │   ├── JwtToken/
│   │   │   │   └── IJwtTokenService.cs     # Interface to sign and verify JWT keys
│   │   │   ├── Localization/
│   │   │   │   └── ILocalizationService.cs # Interface for resource translation
│   │   │   └── Persistence/
│   │   │       ├── IRepository.cs          # Generic repository contract
│   │   │       └── IUnitOfWork.cs          # Unit of Work transaction controller
│   │   ├── Models/
│   │   │   └── TokenResult.cs              # Token DTO for holding token outputs
│   │   ├── Settings/
│   │   │   └── JwtSettings.cs              # Model configuration settings for JWT binding
│   │   └── Validators/
│   │       └── BaseValidator.cs            # Custom generic abstract validator base class
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── DTOs/
│   │   │   │   ├── LoginRequestDto.cs
│   │   │   │   └── RefreshTokenRequestDto.cs
│   │   │   ├── Validators/
│   │   │   │   ├── LoginRequestValidator.cs
│   │   │   │   └── RefreshTokenRequestValidator.cs
│   │   │   └── IAuthService.cs             # Application authorization service contract
│   │   └── JobTitles/
│   │       ├── DTOs/
│   │       │   ├── GetAllJobTitlesInput.cs
│   │       │   ├── JobTitleDto.cs
│   │       │   └── JobTitleInputDto.cs
│   │       ├── Mappings/
│   │       │   └── JobTitleMappingProfile.cs # Mapping mappings for AutoMapper profiles
│   │       ├── Validators/
│   │       └── IJobTitleAppService.cs      # Job Titles service contract
│   ├── Localization/
│   │   └── en/
│   │       └── Validation.xml              # XML dictionary mapping translation keys
│   └── Application.csproj
│
├── Infrastructure/                         # Implementations of External Systems (Depends on Application)
│   ├── Extensions/
│   │   ├── AuthExtensions.cs               # Configure identity and core authentication extensions
│   │   ├── ServiceCollectionExtensions.cs  # Registration helper collection
│   │   └── SettingsExtensions.cs           # Injection helpers mapping section environments
│   └── Services/
│       ├── Auth/
│       │   ├── AuthService.cs              # Authentication logic handler
│       │   ├── CurrentUserContext.cs       # Access system claims identity
│       │   └── CustomJwtBearerEvents.cs    # JWT event callbacks processing pipeline
│       ├── JobTitles/
│       │   └── JobTitleAppService.cs       # Job Titles application services handler
│       ├── Security/
│       └── Common/
│           ├── Helpers/
│           │   └── PasswordHelper.cs       # Argon2/BCrypt hashing and verification logic
│           ├── JwtToken/
│           │   └── JwtTokenService.cs      # Sign access tokens and manage tokens logic
│           └── Localization/
│               └── LocalizationService.cs  # Resource resolver implementation mapping
│   └── Infrastructure.csproj
│
├── Persistence/                            # EF Core configurations & Repositories (Depends on Application)
│   ├── Context/
│   │   └── ApplicationDbContext.cs         # EF Core Database Context implementation
│   ├── Configurations/
│   │   ├── Employees/
│   │   ├── JobTitles/
│   │   │   └── JobTitleConfiguration.cs    # Fluent EF constraints configuration
│   │   ├── UserSessions/
│   │   └── Users/
│   │       └── UserConfiguration.cs        # User table index and property bounds configuration
│   ├── Repositories/
│   │   ├── Repository.cs               # Generic Repository implementation
│   │   └── UnitOfWork.cs               # Transaction logic coordinator wrapper
│   ├── Seeding/
│   │   ├── Employees/
│   │   │   └── EmployeeSeedData.cs         # Predefined employee records database insertion
│   │   ├── JobTitles/
│   │   │   └── JobTitleSeedData.cs         # Primary system roles data insertion
│   │   └── Users/
│   │       └── UserSeedData.cs             # Base defaults superusers seeding config
│   ├── Extensions/
│   │   ├── DatabaseExtensions.cs           # Migration apply and seeding pipeline initializer
│   │   └── DependencyInjection.cs          # Injection dependencies configuration mapping
│   ├── Migrations/                         # Database Migration history log records
│   └── Persistence.csproj
│
├── API/                                    # Controllers and API endpoints entrypoint (Depends on all layers)
│   ├── Controllers/
│   │   ├── Auth/
│   │   │   └── AuthController.cs           # Login, Refresh tokens API endpoints controller
│   │   ├── Common/
│   │   │   └── BaseApiController.cs        # Shared root routing and controller initialization base
│   │   └── JobTitles/
│   │   │   └── JobTitlesController.cs      # Job Titles CRUD operations controller
│   ├── Extensions/
│   │   ├── ApiConfigurationExtensions.cs   # Custom extensions setup mapping pipeline configurations
│   │   ├── ExceptionMapper.cs              # Middleware mapper configuration resolving errors
│   │   ├── MiddlewareExtensions.cs         # Registration utility configuration mapping middlewares
│   │   ├── RateLimitingExtensions.cs       # API client throttling mapping extensions
│   │   ├── SwaggerExtensions.cs            # OpenAPI UI schema specification config
│   │   └── SwaggerMiddlewareExtensions.cs  # Request middleware mappings pipeline for Swagger UI
│   ├── Filters/
│   ├── Middleware/
│   │   ├── GlobalExceptionMiddleware.cs    # Application exception wrapper handler middleware
│   │   ├── RequestMiddleware.cs            # Incoming request auditing logger middleware
│   │   ├── ResponseMiddleware.cs           # Custom processing mapping response headers middleware
│   │   └── SecurityHeadersMiddleware.cs    # Secure injection header parameters wrapper middleware
│   ├── Properties/
│   │   └── launchSettings.json             # Profiles config for visual studio
│   ├── appsettings.json                    # Configuration settings mapping values
│   ├── appsettings.Development.json        # Local environment developer variables
│   └── Program.cs                          # Application entry point, services & middleware pipeline setup
│   └── API.csproj
│
├── Shared/                                 # Common Types & Exceptions (Cross-cutting utility helpers)
│   ├── Constants/
│   │   ├── ErrorCodes.cs                   # Enterprise translation lookup codes
│   │   └── RateLimitPolicies.cs            # Policy names keys collection variables
│   ├── Exceptions/
│   │   ├── AppException.cs                 # Base enterprise exceptions mapping structure
│   │   ├── ForbiddenException.cs           # Handled forbidden operations mapping
│   │   ├── NotFoundException.cs            # Handled resource miss patterns
│   │   ├── UnauthorizedException.cs        # Handled auth miss exception mapping
│   │   └── ValidationException.cs          # Custom validation list tracker mapper
│   ├── Models/
│   │   ├── AuditableEntityDto.cs           # Generic audit values tracker parameters
│   │   ├── FilterRequestDto.cs             # Generic query filter input parameters
│   │   ├── PagedResultDto.cs               # Paginated pagination index outputs DTO
│   │   └── ValidationError.cs              # Custom FluentValidation structure for response wrapping
│   ├── Results/
│   │   ├── ErrorResponse.cs                # Unified error response formatting structure
│   │   ├── PaginatedResult.cs              # Generic wrapper resolving paged records output
│   │   └── Result.cs                       # Generic result response pattern wrapper logic
│   └── Shared.csproj
```
