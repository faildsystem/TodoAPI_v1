using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Dtos.TodoItem;
using TodoAPI.Helpers;
using TodoAPI.interfaces;
using TodoAPI.Mappers;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoItemRepository _repository;

        public TodoController(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todos = await _repository.GetAllAsync(query);

            var todoDto = todos.Select(x => x.ToTodoDto());

            return todoDto.Any() ? Ok(todoDto) : NoContent();

        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var todo = await _repository.GetTodoItemByIdAsync(id);

            return todo == null ? NotFound() : Ok(todo.ToTodoDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CreateTodoItemRequestDto TodoItemDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var TodoItem = TodoItemDto.ToTodoItemFromCreateDto();

            TodoItem.IsCompleted = false;

            await _repository.AddTodoItemAsync(TodoItem);
            return CreatedAtAction(nameof(GetById), new { id = TodoItem.Id }, TodoItem.ToTodoDto());
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var todo = await _repository.DeleteTodoItemAsync(id);

            return todo == null ? NotFound() : NoContent();

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemRequestDto updateTodoItemDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var todo = await _repository.UpdateTodoItemAsync(id, updateTodoItemDto);
            
            return todo == null ? NotFound() : Ok(todo.ToTodoDto());
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCompleted()
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var completedTasks = await _repository.DeleteCompleted();

            return completedTasks == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var todo = await _repository.ChangeStatusAsync(id);
            
            return todo == null ? NotFound() : Ok(todo.ToTodoDto());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCounts()
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Dictionary<string, int> counts = await _repository.GetCountsAsync();
            return Ok(counts);
        }
    }
}
