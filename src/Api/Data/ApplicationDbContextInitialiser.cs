using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.Api.Entities;
using ToDo.Api.Identity;

namespace ToDo.Api.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitialiser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ApplicationDbContextInitialiser> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default user
        var user = new ApplicationUser { UserName = "user@test.com", Email = "user@test.com" };

        if (_userManager.Users.All(u => u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(user, "Password12345!");
        }
        
        // Seed categories
        if (!await _context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Programming",
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Interested In",
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Deep Dive",
                    UserId = user.Id
                }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        // Default todo items
        if (!_context.TodoItems.Any())
        {
            var todoItems = new List<TodoItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Implement User Authentication",
                    Description = "Add JWT authentication with refresh tokens",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Set up EF Core Seeding",
                    Description = "Create proper database seed data for development",
                    Completed = true,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Optimize database queries",
                    Description = "Add indexes and review N+1 problems",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Learn Minimal APIs",
                    Description = "Refactor existing controllers to Minimal APIs",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Create global exception handler",
                    Description = "Implement middleware for consistent error responses",
                    Completed = true,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Build TodoItem-Category many-to-many relationship",
                    Description = "Configure join table and navigation properties",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Write unit tests for TodoService",
                    Description = "Cover CRUD operations with xUnit and Moq",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Implement pagination for Todo list",
                    Description = "Add support for page size and page number",
                    Completed = true,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Dockerize the application",
                    Description = "Create Dockerfile and docker-compose for local development",
                    Completed = false,
                    UserId = user.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Study CQRS pattern",
                    Description = "Explore MediatR for command/query separation",
                    Completed = false,
                    UserId = user.Id
                }
            };

            await _context.TodoItems.AddRangeAsync(todoItems);
            await _context.SaveChangesAsync();
        }
    }
}