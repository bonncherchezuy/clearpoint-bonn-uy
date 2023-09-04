using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.DbContexts;
using TodoList.Api.Entities;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests.Services
{

    public class TodoItemsControllerTest
    {

        #region GetTodoList
        [Fact]
        public async Task When_Calling_GetTodoItems_GetTodoListAsync_Must_Be_Called_Once()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            var actionResult = await sut.GetTodoItems();

            // Assert
            todoItemServiceMock.Verify(x => x.GetTodoListAsync(), Times.Once);
        }

        [Fact]
        public async Task When_Calling_GetTodoItems_MapTodoItemListWithTodoItemDto_Must_Be_Called_Once()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            var actionResult = await sut.GetTodoItems();

            // Assert
            todoItemServiceMock.Verify(x => x.MapTodoItemListWithTodoItemDto(It.IsAny<IEnumerable<TodoItem>>()), Times.Once);
        }


        [Fact]
        public async Task When_Calling_TodoList_Only_Not_Completed_Items_Must_Be_Returned()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();

            var todoItemService = new TodoItemService(context);

            // Act
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);
            var actionResult = await sut.GetTodoItems();
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as IEnumerable<TodoItemDto>;

            // Assert
            Assert.Equal(3, actual.Count());
        }

        #endregion

        #region GetTodoItem
        [Fact]
        public async Task When_Calling_GetTodoItem_GetTodoItemAsync_Must_Be_Called_Once()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            var actionResult = await sut.GetTodoItem(id);

            // Assert
            todoItemServiceMock.Verify(x => x.GetTodoItemAsync(id), Times.Once);
        }

        [Fact]
        public async Task When_Calling_GetTodoItem_MapTodoItemWithTodoItemDto_Must_Be_Called_Once()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            todoItemServiceMock.Setup(i => i.GetTodoItemAsync(id)).ReturnsAsync(
            new TodoItem()
            {
                Id = id,
                Description = "Go for a walk",
                IsCompleted = false
            });
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            var actionResult = await sut.GetTodoItem(id);

            // Assert
            todoItemServiceMock.Verify(x => x.MapTodoItemWithTodoItemDto(It.IsAny<TodoItem>()), Times.Once);
        }

        [Fact]
        public async Task When_Getting_Exisisting_TodoItem_It_Must_Be_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();

            var todoItemService = new TodoItemService(context);

            // Act
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);
            var actionResult = await sut.GetTodoItem(id);
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as TodoItemDto;

            // Assert
            Assert.True(actual.Id == id && actual.Description == "todo item 1" && actual.IsCompleted == false);
        }

        [Fact]
        public async Task When_Getting_Non_Exisisting_TodoItem_NotFoundResult_Must_Be_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);

            // Act

            var actionResult = await sut.GetTodoItem(id);
            var result = actionResult.Result;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region UpdateTodoItem
        [Fact]
        public async Task When_Updating_TodoItem_GetTodoItemAsync_Must_Be_Called_Once()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto()
            {
                Description = "Updated description",
                IsCompleted = true
            };

            // Act
            var actionResult = await sut.UpdateTodoItem(id, updateInfo);

            // Assert
            todoItemServiceMock.Verify(x => x.GetTodoItemAsync(id), Times.Once);
        }

        [Fact]
        public async Task When_Updating_TodoItem_MapTodoItemForUpdateDtoWithTodoItem_Must_Be_Called_Once()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            todoItemServiceMock.Setup(i => i.GetTodoItemAsync(id)).ReturnsAsync(
                new TodoItem()
                {
                    Id = id,
                    Description = "Go for a walk",
                    IsCompleted = false
                });

            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto()
            {
                Description = "Updated description",
                IsCompleted = true
            };

            // Act
            var actionResult = await sut.UpdateTodoItem(id, updateInfo);

            // Assert
            todoItemServiceMock.Verify(x => x.MapTodoItemForUpdateDtoWithTodoItem(It.IsAny<TodoItem>(),It.IsAny<TodoItemForUpdateDto>()), Times.Once);
        }

        [Fact]
        public async Task When_Updating_TodoItem_SaveChangesAsync_Must_Be_Called_Once()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            todoItemServiceMock.Setup(i => i.GetTodoItemAsync(id)).ReturnsAsync(
                new TodoItem()
                {
                    Id = id,
                    Description = "Go for a walk",
                    IsCompleted = false
                });

            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto()
            {
                Description = "Updated description",
                IsCompleted = true
            };

            // Act
            var actionResult = await sut.UpdateTodoItem(id, updateInfo);

            // Assert
            todoItemServiceMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task When_Updating_Non_Exisisting_TodoItem_NotFoundResult_Must_Be_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto() 
            {
                Description = "Updated Description",
                IsCompleted = true,
            };

            // Act
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);
            var actionResult = await sut.UpdateTodoItem(id, updateInfo);
            var result = actionResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task When_Updating_TodoItem_With_Missing_Required_Field_BadRequestResult_Must_Be_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto()
            {
                Description = "",
                IsCompleted = true,
            };
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);
            sut.ModelState.AddModelError("Description", "Required");

            // Act
            var result = await sut.UpdateTodoItem(id, updateInfo);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_Successful_Update_Of_TodoItem_NoContentResult_Must_Be_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            TodoItemForUpdateDto updateInfo = new TodoItemForUpdateDto()
            {
                Description = "New Description",
                IsCompleted = true,
            };
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);

            // Act
            var result = await sut.UpdateTodoItem(id, updateInfo);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region CreateTodoItem

        [Fact]
        public async Task When_Creating_TodoItem_It_Must_Invoke_CreateTodoItemService_Once()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            await sut.CreateTodoItem(new TodoItemForCreationDto { Description = "test 1", IsCompleted = true });

            // Assert
            todoItemServiceMock.Verify( x => x.CreateTodoItem(It.Is<TodoItem>( item => item.Description == "test 1" && item.IsCompleted == true)), Times.Once);
        }

        [Fact]
        public async Task When_Creating_TodoItem_It_SavesToDB()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            await sut.CreateTodoItem(new TodoItemForCreationDto { Description = "test 1", IsCompleted = true });

            // Assert
            todoItemServiceMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task When_Creating_TodoItem_Created_Item_Must_Be_Returned()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);

            // Act
            var actionResult = await sut.CreateTodoItem(new TodoItemForCreationDto { Description = "test 1", IsCompleted = true });
            var result = actionResult.Result as CreatedAtActionResult;
            var actual = result.Value as TodoItem;

            // Assert
            Assert.True(actual.Description == "test 1" && actual.IsCompleted == true);
        }

        [Fact]
        public async Task When_Creating_TodoItem_With_Missing_Required_Field_Return_BadRequest()
        {
            // Arrange
            var todoItemServiceMock = new Mock<ITodoItemService>();
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var sut = new TodoItemsController(todoItemServiceMock.Object, iLoggerMock.Object);
            sut.ModelState.AddModelError("Description", "Required");

            // Act
            var actionResult = await sut.CreateTodoItem(new TodoItemForCreationDto { Description = "", IsCompleted = true });
            var result = actionResult.Result;

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_Creating_TodoItem_With_The_Same_Description_As_One_Of_Not_Completed_Todo_Return_BadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            TodoItemForCreationDto createInfo = new TodoItemForCreationDto()
            {
                Description = "todo item 2",
                IsCompleted = false,
            };
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);

            // Act
            var actionResult = await sut.CreateTodoItem(createInfo);
            var result = actionResult.Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task When_Creating_TodoItem_With_The_Same_Description_As_One_Of_Completed_Todo_Allow_Creation()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var iLoggerMock = new Mock<ILogger<TodoItemsController>>();
            var todoItemService = new TodoItemService(context);
            TodoItemForCreationDto createInfo = new TodoItemForCreationDto()
            {
                Description = "todo item 3",
                IsCompleted = false,
            };
            var sut = new TodoItemsController(todoItemService, iLoggerMock.Object);

            // Act
            var actionResult = await sut.CreateTodoItem(createInfo);
            var result = actionResult.Result as CreatedAtActionResult;
            var actual = result.Value as TodoItem;

            // Assert
            Assert.True(actual.Description == "todo item 3" && actual.IsCompleted == false);
        }

        #endregion

        private async Task<TodoContext> CreateDummyDBContext(Guid id)
        {
            var options = new DbContextOptionsBuilder<TodoContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new TodoContext(options);

            context.Database.EnsureCreated();

            context.TodoItems.Add(new TodoItem() { Id = id, Description = "todo item 1", IsCompleted = false });
            context.TodoItems.Add(new TodoItem() { Id = Guid.NewGuid(), Description = "todo item 2", IsCompleted = false });
            context.TodoItems.Add(new TodoItem() { Id = Guid.NewGuid(), Description = "todo item 3", IsCompleted = true });
            context.TodoItems.Add(new TodoItem() { Id = Guid.NewGuid(), Description = "todo item 4", IsCompleted = false });
            await context.SaveChangesAsync();

            return context;
        }

    }
}
