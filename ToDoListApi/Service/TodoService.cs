using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListAPI.Models;
using ToDoListAPI.Repositories;

namespace ToDoListAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoItem> GetTodoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddTodoAsync(TodoItem todoItem)
        {
            await _repository.AddAsync(todoItem);
        }

        public async Task UpdateTodoAsync(TodoItem todoItem)
        {
            await _repository.UpdateAsync(todoItem);
        }

        public async Task DeleteTodoAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}