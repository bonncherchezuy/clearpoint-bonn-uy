using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Entities;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetTodoListAsync();
        Task<TodoItem> GetTodoItemAsync(Guid id);
        void CreateTodoItem(TodoItem item);
        Task<bool> SaveChangesAsync();
        bool TodoItemDescriptionExists(string description);
        List<TodoItemDto> MapTodoItemListWithTodoItemDto(IEnumerable<TodoItem> todolist);
        TodoItemDto MapTodoItemWithTodoItemDto(TodoItem todoItem);
        TodoItem MapTodoItemForUpdateDtoWithTodoItem(TodoItem todoItem, TodoItemForUpdateDto todoItemForUpdateDto);

    }
}
