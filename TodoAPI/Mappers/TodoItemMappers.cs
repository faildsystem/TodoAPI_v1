using TodoAPI.Dtos.TodoItem;
using TodoAPI.Models;

namespace TodoAPI.Mappers
{
    public static class TodoItemMappers
    {
        public static TodoItemDto ToTodoDto(this TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
                CreatedAt = todoItem.CreatedAt,
                DueDate = todoItem.DueDate
            };
        }

        public static TodoItem ToTodoItemFromCreateDto(this CreateTodoItemRequestDto TodoItemDto)
        {
            return new TodoItem {
                Title = TodoItemDto.Title,
                Description = TodoItemDto.Description,
                DueDate = TodoItemDto.DueDate
            };
        }
    }
}
