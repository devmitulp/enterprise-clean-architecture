# Clean Architecture - ASP.NET Core Backend

A production-ready enterprise Clean Architecture template built with **ASP.NET Core 10**, featuring JWT Authentication, Entity Framework Core, XML-based Localization, and scalable modular architecture.

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [Clean Architecture Layers](#-clean-architecture-layers)
- [Configuration](#-configuration)
- [JWT Authentication](#-jwt-authentication)
- [Database Setup](#-database-setup-with-ef-core)
- [XML-based Localization](#-xml-based-localization)
- [API Documentation](#-api-documentation)
- [Testing](#-testing)
- [Deployment](#-deployment)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### Backend Features
- ✅ **Clean Architecture** - Separation of concerns with 4 distinct layers
- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **Entity Framework Core** - ORM with code-first migrations
- ✅ **Repository Pattern** - Data access abstraction layer
- ✅ **Dependency Injection** - Built-in IoC container
- ✅ **XML Localization** - Multi-language support with XML resource files
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Global Exception Handling** - Centralized error management
- ✅ **CORS Support** - Cross-Origin Resource Sharing configuration
- ✅ **Input Validation** - FluentValidation integration
- ✅ **Async/Await** - Non-blocking operations
- ✅ **Unit Testing** - xUnit test framework setup
- ✅ **Docker Support** - Containerized deployment

---

## 🛠 Tech Stack

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| ASP.NET Core | 10.x | Web Framework |
| C# | Latest | Programming Language |
| Entity Framework Core | 10.x | ORM & Database Access |
| SQL Server | - | Database |
| JWT Bearer | Latest | Authentication |
| Swagger/Swashbuckle | Latest | API Documentation |
| FluentValidation | Latest | Input Validation |
| AutoMapper | Latest | Object Mapping |
| xUnit | Latest | Unit Testing |
| Moq | Latest | Mocking Framework |

---

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK 10.0** or higher ([Download](https://dotnet.microsoft.com/download))
- **Visual Studio 2022** or **Visual Studio Code** with C# extension
- **SQL Server Express** or **PostgreSQL**
- **Git**
- **Docker** (optional, for containerized deployment)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/devmitulp/enterprise-clean-architecture.git
cd enterprise-clean-architecture
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CleanArchDb;Integrated Security=true;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-characters-long",
    "Issuer": "YourAppIssuer",
    "Audience": "YourAppAudience",
    "ExpirationMinutes": 60
  },
  "Localization": {
    "SupportedCultures": ["en-US", "es-ES", "fr-FR", "de-DE"],
    "DefaultCulture": "en-US",
    "ResourcePath": "Resources"
  }
}
```

### 4. Apply Database Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001`

---

## 📁 Project Structure

```
enterprise-clean-architecture/
├── src/
│   ├── API/                          # Presentation Layer (API Controllers)
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Application/                  # Application Layer (Business Logic)
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   ├── MappingProfiles/
│   │   ├── Validators/
│   │   └── Exceptions/
│   │
│   ├── Domain/                       # Domain Layer (Entities & Business Rules)
│   │   ├── Entities/
│   │   ├── Events/
│   │   ├── ValueObjects/
│   │   └── Enums/
│   │
│   └── Infrastructure/               # Infrastructure Layer (External Services)
│       ├── Persistence/
│       │   ├── Context/
│       │   ├── Repositories/
│       │   └── Migrations/
│       ├── Services/
│       ├── Authentication/
│       └── Localization/
│
├── tests/
│   ├── Application.Tests/            # Application Layer Tests
│   ├── API.Tests/                    # API Integration Tests
│   └── Infrastructure.Tests/         # Infrastructure Tests
│
└── docker-compose.yml
```

---

## 🏗 Clean Architecture Layers

### 1. **Domain Layer** (Core Business Logic)
- Entities: `User`, `Product`, `Order`, etc.
- Value Objects: `Email`, `Price`, `Address`, etc.
- Interfaces: Business contracts
- Business rules independent of frameworks

```csharp
public class User : AggregateRoot
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 2. **Application Layer** (Use Cases & Business Logic)
- DTOs: Data Transfer Objects
- Services: Business logic implementation
- Interfaces: Service contracts
- Validators: Input validation

```csharp
public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    
    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User { Email = request.Email, FirstName = request.FirstName };
        await _repository.AddAsync(user);
        return MapToDto(user);
    }
}
```

### 3. **Infrastructure Layer** (External Services & Data Access)
- Repository Pattern: Data persistence
- Entity Framework Core: ORM
- Authentication: JWT implementation
- Localization: XML resource management

```csharp
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
```

### 4. **Presentation Layer** (API Controllers)
- REST endpoints
- Request/Response handling
- Exception handling middleware

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
    }
}
```

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CleanArchDb;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-that-is-at-least-32-characters",
    "Issuer": "YourApp",
    "Audience": "YourAppUsers",
    "ExpirationMinutes": 120
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

Set environment-specific configurations:

```bash
# Development
set ASPNETCORE_ENVIRONMENT=Development

# Production
set ASPNETCORE_ENVIRONMENT=Production
```

---

## 🔐 JWT Authentication

### Setup in Program.cs

```csharp
var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["JwtSettings:Audience"],
        ValidateLifetime = true
    };
});

app.UseAuthentication();
app.UseAuthorization();
```

### Generate JWT Token

```csharp
public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName)
        };
        
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["JwtSettings:ExpirationMinutes"])),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### Using Protected Endpoints

```csharp
[Authorize]
[HttpGet("{id}")]
public async Task<IActionResult> GetUser(int id)
{
    var user = await _userService.GetUserByIdAsync(id);
    return Ok(user);
}
```

---

## 💾 Database Setup with EF Core

### Create DbContext

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
    }
}
```

### Migrations

```bash
# Create a new migration
dotnet ef migrations add AddUserTable

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Program.cs Configuration

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
```

---

## 🌍 XML-based Localization

This project uses **XML resource files** for multi-language support.

### XML Resource File Structure

Create resource files in `Resources` folder:

**Resources/Localization/en-US.xml**
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <strings>
    <string key="Welcome">Welcome to our application</string>
    <string key="Goodbye">Thank you for using our service</string>
    <string key="InvalidEmail">Please enter a valid email address</string>
    <string key="UserNotFound">User not found</string>
    <string key="UnauthorizedAccess">You do not have permission to access this resource</string>
    <string key="Success">Operation completed successfully</string>
    <string key="Error">An error occurred while processing your request</string>
  </strings>
</root>
```

**Resources/Localization/es-ES.xml**
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <strings>
    <string key="Welcome">Bienvenido a nuestra aplicación</string>
    <string key="Goodbye">Gracias por usar nuestro servicio</string>
    <string key="InvalidEmail">Por favor, ingrese una dirección de correo válida</string>
    <string key="UserNotFound">Usuario no encontrado</string>
    <string key="UnauthorizedAccess">No tiene permiso para acceder a este recurso</string>
    <string key="Success">Operación completada exitosamente</string>
    <string key="Error">Ocurrió un error al procesar su solicitud</string>
  </strings>
</root>
```

**Resources/Localization/fr-FR.xml**
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <strings>
    <string key="Welcome">Bienvenue dans notre application</string>
    <string key="Goodbye">Merci d'avoir utilisé notre service</string>
    <string key="InvalidEmail">Veuillez entrer une adresse email valide</string>
    <string key="UserNotFound">Utilisateur non trouvé</string>
    <string key="UnauthorizedAccess">Vous n'avez pas la permission d'accéder à cette ressource</string>
    <string key="Success">Opération complétée avec succès</string>
    <string key="Error">Une erreur s'est produite lors du traitement de votre demande</string>
  </strings>
</root>
```

### Localization Service Implementation

```csharp
public interface ILocalizationService
{
    string GetString(string key, string culture = null);
    Dictionary<string, string> GetAllStrings(string culture = null);
}

public class LocalizationService : ILocalizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _resourcePath;
    private Dictionary<string, Dictionary<string, string>> _resources;
    
    public LocalizationService(IHttpContextAccessor httpContextAccessor, 
        IConfiguration config)
    {
        _httpContextAccessor = httpContextAccessor;
        _resourcePath = Path.Combine(Directory.GetCurrentDirectory(), 
            config["Localization:ResourcePath"]);
        LoadResources();
    }
    
    public string GetString(string key, string culture = null)
    {
        if (string.IsNullOrEmpty(culture))
        {
            culture = _httpContextAccessor.HttpContext?.GetRouteValue("culture") 
                as string ?? "en-US";
        }
        
        if (_resources.TryGetValue(culture, out var cultureDictionary))
        {
            if (cultureDictionary.TryGetValue(key, out var value))
            {
                return value;
            }
        }
        
        return key; // Return key if not found
    }
    
    public Dictionary<string, string> GetAllStrings(string culture = null)
    {
        if (string.IsNullOrEmpty(culture))
        {
            culture = _httpContextAccessor.HttpContext?.GetRouteValue("culture") 
                as string ?? "en-US";
        }
        
        return _resources.TryGetValue(culture, out var result) 
            ? result 
            : new Dictionary<string, string>();
    }
    
    private void LoadResources()
    {
        _resources = new Dictionary<string, Dictionary<string, string>>();
        
        if (!Directory.Exists(_resourcePath))
            return;
        
        foreach (var file in Directory.GetFiles(_resourcePath, "*.xml"))
        {
            var culture = Path.GetFileNameWithoutExtension(file);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            
            var cultureDictionary = new Dictionary<string, string>();
            
            foreach (XmlNode node in xmlDoc.SelectNodes("//string"))
            {
                var key = node.Attributes?["key"]?.Value;
                var value = node.InnerText;
                
                if (!string.IsNullOrEmpty(key))
                {
                    cultureDictionary[key] = value;
                }
            }
            
            _resources[culture] = cultureDictionary;
        }
    }
}
```

### Register in Program.cs

```csharp
services.AddHttpContextAccessor();
services.AddSingleton<ILocalizationService, LocalizationService>();

// Add localization configuration
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "es-ES", "fr-FR", "de-DE" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

app.UseRequestLocalization();
```

### Using Localization in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILocalizationService _localization;
    
    public UserController(IUserService userService, 
        ILocalizationService localization)
    {
        _userService = userService;
        _localization = localization;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var result = await _userService.CreateUserAsync(request);
            var message = _localization.GetString("Success");
            return CreatedAtAction(nameof(GetUser), 
                new { id = result.Id, message }, result);
        }
        catch (Exception ex)
        {
            var errorMessage = _localization.GetString("Error");
            return BadRequest(new { message = errorMessage });
        }
    }
}
```

### Query by Culture

Request endpoint with culture parameter:

```
GET /api/user?culture=es-ES
GET /api/user?culture=fr-FR
```

---

## 📚 API Documentation

### Swagger Configuration

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Architecture API",
        Version = "v1",
        Description = "Enterprise-grade API with clean architecture"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API v1");
});
```

Access Swagger UI at: `https://localhost:5001/swagger`

---

## 🧪 Testing

### Unit Testing Example

```csharp
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;
    
    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task CreateUser_WithValidData_ReturnsUserDto()
    {
        // Arrange
        var request = new CreateUserRequest 
        { 
            Email = "test@example.com", 
            FirstName = "John" 
        };
        
        var user = new User 
        { 
            Id = 1, 
            Email = request.Email, 
            FirstName = request.FirstName 
        };
        
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(user);
        
        // Act
        var result = await _userService.CreateUserAsync(request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
```

### Run Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Application.Tests

# Run with verbose output
dotnet test --verbosity detailed
```

---

## 🐳 Deployment

### Docker Setup

**Dockerfile**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/API/API.csproj"

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
ENTRYPOINT ["dotnet", "API.dll"]
```

### Docker Compose

**docker-compose.yml**
```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "5001:5001"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=CleanArchDb;User Id=sa;Password=YourPassword@123;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword@123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

### Build and Run

```bash
docker-compose build
docker-compose up
```

---

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Code Style
- Follow C# naming conventions (PascalCase for public members)
- Use meaningful variable and method names
- Add XML documentation comments for public API
- Keep methods small and focused

---

## 📄 License

This project is licensed under the **MIT License** - see the LICENSE file for details.

---

## 📞 Support

For questions, issues, or suggestions:
- **Issues**: [GitHub Issues](https://github.com/devmitulp/enterprise-clean-architecture/issues)
- **Email**: devmitulp@example.com

---

## 🙏 Acknowledgments

- ASP.NET Core documentation and community
- Clean Architecture principles by Robert C. Martin
- Entity Framework Core team
- All contributors

---

**Happy Coding! 🚀**
