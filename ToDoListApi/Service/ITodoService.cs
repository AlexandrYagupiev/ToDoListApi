using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();
        Task<TodoItem> GetTodoByIdAsync(int id);
        Task AddTodoAsync(TodoItem todoItem);
        Task UpdateTodoAsync(TodoItem todoItem);
        Task DeleteTodoAsync(int id);
    }
}