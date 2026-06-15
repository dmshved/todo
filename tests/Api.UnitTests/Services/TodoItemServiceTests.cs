using FluentAssertions;
using Moq;
using ToDo.Api.Dtos;
using ToDo.Api.Entities;
using ToDo.Api.Exceptions;
using ToDo.Api.Repositories.Abstractions;
using ToDo.Api.Services;
using ToDo.Api.Services.Abstractions;
using Xunit;

namespace Api.UnitTests.Services;

public class TodoItemServiceTests
{
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock = new();
    private readonly Mock<ICategoryService> _categoryServiceMock = new();
    private readonly Mock<IUser> _currentUserMock = new();

    private readonly TodoItemService _todoItemService;

    public TodoItemServiceTests()
    {
        _currentUserMock.Setup(x => x.Id)
            .Returns("12345");

        _todoItemService = new TodoItemService(_todoItemRepositoryMock.Object, _categoryServiceMock.Object, _currentUserMock.Object);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsTodoItem_WhenUserOwnsIt()
    {
        var id = Guid.NewGuid();

        var todoItem = new TodoItem { Id = id, UserId = "12345" };

        _todoItemRepositoryMock.Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(todoItem);

        var result = await _todoItemService.GetByIdAsync(id);

        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenItemMissing()
    {
        var id = Guid.NewGuid();

        _todoItemRepositoryMock.Setup(r => r.GetFirstAsync(id))
            .ReturnsAsync((TodoItem?)null);

        Func<Task> act = async () => await _todoItemService.GetByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task CreateAsync_SavesItem()
    {
        var dto = new CreateTodoItemDto
        (
            Title: "The best programmer in the world comes in the era when people are most fired up of the programming",
            Description: "They come out of nowhere, by their own"
        );

        _todoItemRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TodoItem>()))
            .Returns(Task.CompletedTask);

        _todoItemRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _todoItemService.CreateAsync(dto);
        
        _todoItemRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Once);
        _todoItemRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        result.Data.Title.Should().Be("The best programmer in the world comes in the era when people are most fired up of the programming");
    }
    
    [Fact]
    public async Task ToggleCompleted_TogglesValue()
    {
        var id = Guid.NewGuid();

        var todoItem = new TodoItem
        {
            Id = id,
            UserId = "12345",
            Completed = false
        };

        _todoItemRepositoryMock.Setup(x => x.GetFirstAsync(id))
            .ReturnsAsync(todoItem);
        
        
        _todoItemRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _todoItemService.ToggleCompleted(id);

        result.Data.Completed.Should().BeTrue();
    }
}