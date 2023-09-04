using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.DbContexts;
using TodoList.Api.Entities;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoContext _context;

        public TodoItemService(TodoContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<TodoItem> GetTodoItemAsync(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoListAsync()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public void CreateTodoItem(TodoItem item)
        {
           var x = _context.TodoItems.Add(item);
        }

        public async Task<bool> SaveChangesAsync() 
        {
            return (await _context.SaveChangesAsync() >= 1);
        }

        public bool TodoItemDescriptionExists(string description) 
        {
            return _context.TodoItems.Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }

        public List<TodoItemDto> MapTodoItemListWithTodoItemDto(IEnumerable<TodoItem> todolist) 
        {
            var results = new List<TodoItemDto>();
            foreach (var item in todolist)
            {
                results.Add(new TodoItemDto
                {
                    Id = item.Id,
                    Description = item.Description,
                    IsCompleted = item.IsCompleted
                });
            }
            return results;
        }

        public TodoItemDto MapTodoItemWithTodoItemDto(TodoItem todoItem) 
        {
            return new TodoItemDto { Id = todoItem.Id, Description = todoItem.Description, IsCompleted = todoItem.IsCompleted };
        }

        public TodoItem MapTodoItemForUpdateDtoWithTodoItem(TodoItem todoItem, TodoItemForUpdateDto todoItemForUpdateDto) 
        {
            todoItem.Description = todoItemForUpdateDto.Description;
            todoItem.IsCompleted = todoItemForUpdateDto.IsCompleted;

            return todoItem;
        }
    }
}
