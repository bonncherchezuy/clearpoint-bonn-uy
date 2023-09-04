using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Entities;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemService todoItemService, ILogger<TodoItemsController> logger)
        {
            _todoItemService = todoItemService ?? throw new ArgumentNullException(nameof(todoItemService));
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            _logger.LogInformation("GetTodoItems: Request to get the non-completed todo items was called");
            var todolist = await _todoItemService.GetTodoListAsync();

            var results = _todoItemService.MapTodoItemListWithTodoItemDto(todolist);

            _logger.LogInformation("GetTodoItems: Request Successful");
            return Ok(results);
        }


        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(Guid id)
        {
            _logger.LogInformation($"GetTodoItem: Request to get todo item with id {id}");
            var todoItem = await _todoItemService.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                _logger.LogError($"GetTodoItem: Todo item with id {id} does not exist.");
                return NotFound("todo item does not exist");
            }

            var result = _todoItemService.MapTodoItemWithTodoItemDto(todoItem);
            

            _logger.LogInformation($"GetTodoItem: Successfully get todo item with id {id}");
            return Ok(result);
        }

        // PUT: api/TodoItems/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoItem(Guid id, TodoItemForUpdateDto updatedTodo)
        {
            _logger.LogInformation($"UpdateTodoItem: attempting to update todo item with id {id}");
            var todoItem = await _todoItemService.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                _logger.LogError($"UpdateTodoItem: Todo item with id {id} does not exist.");
                return NotFound("You are updating item that does not exist");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError($"UpdateTodoItem: one or more model state is invalid");
                return BadRequest("Description is required");
            }

            todoItem = _todoItemService.MapTodoItemForUpdateDtoWithTodoItem(todoItem, updatedTodo);

            _logger.LogInformation($"UpdateTodoItem: saving changes on database");
            await _todoItemService.SaveChangesAsync();

            _logger.LogInformation($"UpdateTodoItem: successfully updated todo item with id {id}");
            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItemForCreationDto todoItemForCreationDto)
        {
            _logger.LogInformation($"CreateTodoItem: Attempting to create new todo item");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"CreateTodoItem: one or more model state is invalid");
                return BadRequest("Description is required");
            }
            else if (_todoItemService.TodoItemDescriptionExists(todoItemForCreationDto.Description))
            {
                _logger.LogError($"CreateTodoItem: Creation of todo item failed, Description must be unique for none finish task");
                return BadRequest("Description already exists");
            }

            var newTodoItem = new TodoItem()
            { 
               Description = todoItemForCreationDto.Description,
               IsCompleted = todoItemForCreationDto.IsCompleted
            };

            _todoItemService.CreateTodoItem(newTodoItem);
            await _todoItemService.SaveChangesAsync();
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = newTodoItem.Id }, newTodoItem);
        } 
    }
}
