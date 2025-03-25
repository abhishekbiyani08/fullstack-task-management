using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Models.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _taskService.GetAllForUserAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var task = await _taskService.GetByIdAsync(id, userId);
                return Ok(task);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve task");
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateUpdateDto taskCreateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var createdTask = await _taskService.CreateAsync(taskCreateDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Failed to create task");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskCreateUpdateDto taskUpdateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var updatedTask = await _taskService.UpdateAsync(id, taskUpdateDto, userId);
                return Ok(updatedTask);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Failed to update task");
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var completedTask = await _taskService.CompleteTaskAsync(id, userId);
                return Ok(completedTask);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Failed to complete task");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _taskService.DeleteAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}