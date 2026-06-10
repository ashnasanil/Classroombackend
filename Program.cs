using System.Text;
using GoogleClassroom.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IAuthService, GoogleClassroom.API.Services.Implementations.AuthService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IClassroomService, GoogleClassroom.API.Services.Implementations.ClassroomService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IAnnouncementService, GoogleClassroom.API.Services.Implementations.AnnouncementService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IAssignmentService, GoogleClassroom.API.Services.Implementations.AssignmentService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IFileUploadService, GoogleClassroom.API.Services.Implementations.FileUploadService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IMaterialService, GoogleClassroom.API.Services.Implementations.MaterialService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.INotificationService, GoogleClassroom.API.Services.Implementations.NotificationService>();
builder.Services.AddScoped<GoogleClassroom.API.Services.Interfaces.IEmailService, GoogleClassroom.API.Services.EmailService>();

builder.Services.AddMemoryCache();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Google Classroom API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("Secret");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201", "http://localhost:4206")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var retryCount = 5;
    var currentRetry = 0;
    var delay = TimeSpan.FromSeconds(5);
    
    while (currentRetry < retryCount)
    {
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            logger.LogInformation("Database migration completed successfully.");
            break;
        }
        catch (Exception ex)
        {
            currentRetry++;
            logger.LogWarning(ex, $"An error occurred while migrating the database. Retrying... ({currentRetry}/{retryCount})");
            if (currentRetry == retryCount)
            {
                logger.LogError(ex, "Failed to migrate the database after multiple retries.");
            }
            Thread.Sleep(delay);
        }
    }
}

app.Run();
