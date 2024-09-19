using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Dtos.TodoItem;
using TodoAPI.Helpers;
using TodoAPI.interfaces;
using TodoAPI.Models;

namespace TodoAPI.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoDbContext _context;

        public TodoItemRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> AddTodoItemAsync(TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem?> ChangeStatusAsync(int id)
        {
            var todo = await GetTodoItemByIdAsync(id);
            if (todo == null)
            {
                return null;
            }
            todo.IsCompleted = !todo.IsCompleted;
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<List<TodoItem>?> DeleteCompleted()
        {
            var completed = await _context.TodoItems.Where(x => x.IsCompleted == true).ToListAsync();
            if (completed.Any())
            {
                _context.RemoveRange(completed);
                await _context.SaveChangesAsync();
                return completed;
            }
            return null;

        }

        public async Task<TodoItem?> DeleteTodoItemAsync(int id)
        {
            var todoItem = await GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return null;
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;

        }

        public async Task<List<TodoItem>> GetAllAsync(QueryObject query)
        {
            var todos = _context.TodoItems.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                todos = todos.Where(x => x.Title == query.Title);
            }

            if (query.IsCompleted != null)
            {
                todos = todos.Where(x => x.IsCompleted == query.IsCompleted);
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            
            return await todos.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetCountsAsync()
        {
            int all = await _context.TodoItems.CountAsync();
            int pendings = await _context.TodoItems.Where(x => x.IsCompleted == false).CountAsync();
            int completed = all - pendings;

            Dictionary<string, int> counts =
              new Dictionary<string, int>()
              {
                  {"all", all},
                  {"pendings", pendings},
                  {"completed", completed}
              };
            return counts;
        }

        public async Task<TodoItem?> GetTodoItemByIdAsync(int id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TodoItem?> UpdateTodoItemAsync(int id, UpdateTodoItemRequestDto todoDto)
        {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
            {
                return null;
            }

            todo.Title = todoDto.Title;
            todo.Description = todoDto.Description;
            todo.IsCompleted = todoDto.IsCompleted;
            todo.DueDate = todoDto.DueDate;

            await _context.SaveChangesAsync();
            return todo;
        }

    }
}
