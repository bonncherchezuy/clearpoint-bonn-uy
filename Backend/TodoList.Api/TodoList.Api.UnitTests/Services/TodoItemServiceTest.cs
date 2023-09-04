using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.DbContexts;
using TodoList.Api.Entities;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests.Services
{
    public class TodoItemServiceTest { 

        #region GetTodoItemAsync
        [Fact]
        public async Task When_Existing_TodoItem_Is_Requested_Return_The_Item()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var sut = new TodoItemService(context);

            // Act
            var actual = await sut.GetTodoItemAsync(id);

            // Assert
            Assert.True(actual.Id == id && actual.Description == "todo item 1" && actual.IsCompleted == false);
        }

        [Fact]
        public async Task When_Requested_Data_Does_Not_Exist_Return_Null()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = await sut.GetTodoItemAsync(id);

            // Assert
            Assert.True(actual == null);
        }
        #endregion

        #region GetTodoListAsync
        [Fact]
        public async Task When_Requesting_For_Todo_List_Only_Return_Not_Completed_Task()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = await sut.GetTodoListAsync();

            // Assert
            Assert.True(actual.Count() == 3);
        }

        [Fact]
        public async Task When_Requesting_For_Todo_List_From_Empty_DbContext_Return_Empty_List()
        {
            // Arrange
            var context = await CreateEmptyDBContext();
            var sut = new TodoItemService(context);

            // Act
            var actual = await sut.GetTodoListAsync();

            // Assert
            Assert.True(actual.Count() == 0);
        }
        #endregion

        #region Creation
        [Fact]
        public async Task When_Creating_TodoItem_Count_Should_Increment()
        {
            // Arrange
            var id = Guid.NewGuid();
            var context = await CreateDummyDBContext(id);
            var sut = new TodoItemService(context);
            TodoItem todoItem = new TodoItem()
            {
                Description = "newly added item",
                IsCompleted = false
            };

            // Act
            sut.CreateTodoItem(todoItem);
            var result = await sut.SaveChangesAsync();

            // Assert
            Assert.True(result);
        }
        #endregion

        #region TodoItemDescriptionExists
        [Fact]
        public async Task When_Description_Matches_With_Any_Not_Completed_Item_It_Should_Return_True()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = sut.TodoItemDescriptionExists("todo item 2");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task When_Matching_Description_It_Should_Be_Case_Insensitive()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = sut.TodoItemDescriptionExists("ToDo Item 2");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task When_Description_Matches_With_Any_Completed_Item_It_Should_Return_False()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = sut.TodoItemDescriptionExists("todo item 3");

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task When_Description_Doesnt_Match_With_Any_Item_It_Should_Return_False()
        {
            // Arrange
            var context = await CreateDummyDBContext(Guid.NewGuid());
            var sut = new TodoItemService(context);

            // Act
            var actual = sut.TodoItemDescriptionExists("todo item 7");

            // Assert
            Assert.False(actual);
        }
        #endregion

        #region Mapping
        [Fact]
        public async Task When_Mapping_TodoItem_List_To_TodoItemDto_The_Count_Of_Element_Should_Be_Generated() 
        {
            //Arrange
            List<TodoItem> todolist = new List<TodoItem>() 
            {
                new TodoItem { Id = Guid.NewGuid(), Description = "new todo 1", IsCompleted = false},
                new TodoItem { Id = Guid.NewGuid(), Description = "new todo 2", IsCompleted = false},
                new TodoItem { Id = Guid.NewGuid(), Description = "new todo 3", IsCompleted = false},
                new TodoItem { Id = Guid.NewGuid(), Description = "new todo 4", IsCompleted = false},
            };

            var context = await CreateEmptyDBContext();
            var sut = new TodoItemService(context);

            //Act
            var result = sut.MapTodoItemListWithTodoItemDto(todolist);

            //Assert
            Assert.Equal(todolist.Count, result.Count);
        }

        [Fact]
        public async Task When_Mapping_TodoItem_To_TodoItemDto_Their_Values_Should_Be_The_Same()
        {
            //Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "new todo 1", IsCompleted = false };
            var context = await CreateEmptyDBContext();
            var sut = new TodoItemService(context);

            //Act
            var result = sut.MapTodoItemWithTodoItemDto(todoItem);

            //Assert
            Assert.True(result.Id == todoItem.Id && result.Description == todoItem.Description && result.IsCompleted == todoItem.IsCompleted);
        }

        [Fact]
        public async Task When_Mapping_TodoItemForUpdateDto_To_TodoItem_It_Should_Update_TodoItem_Value()
        {
            //Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "walk the dog", IsCompleted = false };
            var todoItemForUpdate = new TodoItemForUpdateDto { Description = "run 10 KM", IsCompleted = true };
            var context = await CreateEmptyDBContext();
            var sut = new TodoItemService(context);

            //Act
            var result = sut.MapTodoItemForUpdateDtoWithTodoItem(todoItem, todoItemForUpdate);

            //Assert
            Assert.True(result.Id == todoItem.Id && result.Description == todoItemForUpdate.Description && result.IsCompleted == todoItemForUpdate.IsCompleted);
        }
        #endregion

        #region private method
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

        private async Task<TodoContext> CreateEmptyDBContext()
        {
            var options = new DbContextOptionsBuilder<TodoContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new TodoContext(options);

            context.Database.EnsureCreated();

            await context.SaveChangesAsync();

            return context;
        }

        #endregion
    }
}
