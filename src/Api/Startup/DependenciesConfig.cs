using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using ToDo.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ToDo.Api.Data.Options;
using ToDo.Api.Data.Setup;
using ToDo.Api.Exceptions;
using ToDo.Api.Identity;
using ToDo.Api.Repositories;
using ToDo.Api.Repositories.Abstractions;
using ToDo.Api.Services;
using ToDo.Api.Services.Abstractions;
using ToDo.Api.Shared;

namespace ToDo.Api.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ApplicationConstants.Database);
        Guard.Against.Null(connectionString, message: $"Connection string '{ApplicationConstants.Database}' not found.");
      
        // Application databse context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        // Jwt config with Options pattern
        builder.Services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName);
        
        builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        builder.Services.AddAuthorizationBuilder();
        
        // Database initialiser
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();
        
        // Repositories
        builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        // Services
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ITodoItemService, TodoItemService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        // Current user
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUser, CurrentUserService>();
       
        // Problem deatails exception handler
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        
        // Controllers
        builder.Services.AddControllers();
       
        // CORS
        builder.Services.AddCorsServices();
        
        // OpenApi
        builder.Services.AddOpenApiServices();
        
        // Routing
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        }); 
    }
}