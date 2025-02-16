using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoListAPI.Controllers;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using Xunit;

namespace ToDoListAPITests
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _mockService;
        private readonly TodosController _controller;

        public TodosControllerTests()
        {
            _mockService = new Mock<ITodoService>();
            _controller = new TodosController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithItems()
        {
            
            var items = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Task 1",Description="Description 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Task 2",Description="Description 2", IsCompleted = true }
            };
            _mockService.Setup(service => service.GetAllTodosAsync()).ReturnsAsync(items);

            
            var result = await _controller.GetAllAsync();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItems = Assert.IsAssignableFrom<List<TodoItem>>(okResult.Value);
            Assert.Equal(2, returnedItems.Count);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundResult_WhenInvalidId()
        {
            
            _mockService.Setup(service => service.GetTodoByIdAsync(It.IsAny<int>())).ReturnsAsync((TodoItem)null);

            
            var result = await _controller.GetByIdAsync(99);

            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithCorrectItem()
        {
            
            var testItem = new TodoItem { Id = 1, Title = "Task 1", Description = "Description 1", IsCompleted = false };
            _mockService.Setup(service => service.GetTodoByIdAsync(testItem.Id)).ReturnsAsync(testItem);

            
            var result = await _controller.GetByIdAsync(testItem.Id);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(testItem.Id, returnedItem.Id);
        }

        [Fact]
        public async Task Post_CreateNewItem_ReturnsCreatedAtActionResult()
        {
            
            var newItem = new TodoItem { Title = "New Task", Description = "Description", IsCompleted = false };
            _mockService.Setup(service => service.AddTodoAsync(newItem)).Returns(Task.CompletedTask);

            
            var result = await _controller.CreateAsync(newItem);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedItem = Assert.IsType<TodoItem>(createdResult.Value);
            Assert.Equal(newItem.Title, returnedItem.Title);
        }

        [Fact]
        public async Task Put_UpdateExistingItem_ReturnsNoContentResult()
        {
            
            var updatedItem = new TodoItem { Id = 1, Title = "Updated Task", Description = "Description", IsCompleted = true };
            _mockService.Setup(service => service.UpdateTodoAsync(updatedItem)).Returns(Task.CompletedTask);

            
            var result = await _controller.UpdateAsync(updatedItem.Id, updatedItem);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_RemoveExistingItem_ReturnsNoContentResult()
        {
            
            var itemId = 1;
            _mockService.Setup(service => service.DeleteTodoAsync(itemId)).Returns(Task.CompletedTask);

            
            var result = await _controller.DeleteAsync(itemId);

            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}