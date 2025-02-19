using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodosController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllAsync()
        {
            var todos = await _service.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetByIdAsync(int id)
        {
            var todo = await _service.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddTodoAsync(todoItem);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TodoItem todoItem)
        {
            if (id != todoItem.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateTodoAsync(todoItem);
            }
            catch
            {
                if (!await TodoExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await TodoExistsAsync(id))
            {
                return NotFound();
            }

            await _service.DeleteTodoAsync(id);
            return NoContent();
        }

        private async Task<bool> TodoExistsAsync(int id)
        {
            var todo = await _service.GetTodoByIdAsync(id);
            return todo != null;
        }
    }
}