using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ToDo.Api.Dtos;
using ToDo.Api.Entities;
using ToDo.Api.Exceptions;
using ToDo.Api.Repositories.Abstractions;
using ToDo.Api.Services;
using ToDo.Api.Services.Abstractions;
using Xunit;

namespace Api.UnitTests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRespoitoryMock;
    private readonly Mock<IUser> _currentUserMock;
    
    private readonly CategoryService _categoryService;
    
    public CategoryServiceTests()
    {
        _categoryRespoitoryMock = new Mock<ICategoryRepository>();
        _currentUserMock = new Mock<IUser>();

        _currentUserMock.Setup(x => x.Id)
            .Returns("12345");

        _categoryService = new CategoryService(_categoryRespoitoryMock.Object, _currentUserMock.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsCategories()
    {
        var categories = new List<Category>
        {
            new (){ Id = Guid.NewGuid(), Name = "Programming", UserId = "12345"}
        };

        _categoryRespoitoryMock.Setup(x => x.GetAllAsync("12345"))
            .ReturnsAsync(categories);

        var result = await _categoryService.GetAsync();

        result.Data.First().Name.Should().Be("Programming");
        
        _categoryRespoitoryMock.Verify(x => x.GetAllAsync("12345"), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCategory_WhenUserOwnsCategory()
    {
        var id = Guid.NewGuid();

        var category = new Category
        {
            Id = id,
            Name = "Developer",
            UserId = "12345",
        };

        _categoryRespoitoryMock.Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(category);

        var result = await _categoryService.GetByIdAsync(id);

        result.Data.Should().Be(category);

        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenCategoryMissing()
    {
        var id = Guid.NewGuid();

        _categoryRespoitoryMock
            .Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync((Category?)null);

        Func<Task> action = async () => await _categoryService.GetByIdAsync(id);

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task GetByIdAsync_ThrowsUnauthorizedException_WhenCategoryBelongsToAnotherUser()
    {
        var id = Guid.NewGuid();

        var category = new Category
        {
            Id = id,
            UserId = "another-user"
        };

        _categoryRespoitoryMock
            .Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(category);

        Func<Task> action = async () =>
            await _categoryService.GetByIdAsync(id);

        await action.Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Fact]
    public async Task CreateAsync_CreatesCategory()
    {
        var dto = new CreateCategoryDto("Deep dive");

        _categoryRespoitoryMock
            .Setup(x => x.AddAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);

        _categoryRespoitoryMock
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _categoryService.CreateAsync(dto);

        result.Data.Name
            .Should()
            .Be("Deep dive");

        result.StatusCode
            .Should()
            .Be(StatusCodes.Status201Created);

        _categoryRespoitoryMock.Verify(
            x => x.AddAsync(It.IsAny<Category>()),
            Times.Once);

        _categoryRespoitoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_UpdatesCategory()
    {
        var id = Guid.NewGuid();
        
        var category = new Category
        {
            Id = id,
            UserId = "12345",
            Name = "ASP.NET"
        };

        _categoryRespoitoryMock
            .Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(category);

        _categoryRespoitoryMock
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var dto = new UpdateCategoryDto(Name: "ASP.NET Core");

        var result = await _categoryService.UpdateAsync(id, dto);

        result.Data.Name
            .Should()
            .Be("ASP.NET Core");
        
        _categoryRespoitoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_DeletesCategory()
    {
        var id = Guid.NewGuid();
        
        var category = new Category
        {
            Id = id,
            UserId = "12345"
        };

        _categoryRespoitoryMock
            .Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(category);

        _categoryRespoitoryMock
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _categoryService.DeleteAsync(id);
        
        _categoryRespoitoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
