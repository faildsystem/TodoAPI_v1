using TodoAPI.Dtos.TodoItem;
using TodoAPI.Helpers;
using TodoAPI.Models;

namespace TodoAPI.interfaces
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllAsync(QueryObject query);

        Task<TodoItem?> GetTodoItemByIdAsync(int id);

        Task<TodoItem> AddTodoItemAsync(TodoItem todoItem);

        Task<TodoItem?> DeleteTodoItemAsync(int id);

        Task<TodoItem?> UpdateTodoItemAsync(int id, UpdateTodoItemRequestDto todoDto);

        Task<List<TodoItem>?> DeleteCompleted();

        Task<TodoItem?> ChangeStatusAsync(int id);

        Task<Dictionary<string, int>> GetCountsAsync();

    }
}
