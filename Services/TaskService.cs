using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Data.Repositories;
using TaskManagementApi.Models;
using TaskManagementApi.Models.DTOs;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllForUserAsync(int userId)
        {
            var tasks = await _taskRepository.GetAllForUserAsync(userId);
            var taskDtos = new List<TaskDto>();

            foreach (var task in tasks)
            {
                taskDtos.Add(MapToTaskDto(task));
            }

            return taskDtos;
        }

        public async Task<TaskDto> GetByIdAsync(int id, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                throw new ApplicationException("Task not found");

            return MapToTaskDto(task);
        }

        public async Task<TaskDto> CreateAsync(TaskCreateUpdateDto taskCreateDto, int userId)
        {
            var task = new UserTask
            {
                Title = taskCreateDto.Title!,
                Description = taskCreateDto.Description!,
                UserId = userId,
                StartDate = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return MapToTaskDto(task);
        }

        public async Task<TaskDto> UpdateAsync(int id, TaskCreateUpdateDto taskUpdateDto, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                throw new ApplicationException("Task not found");

            task.Title = taskUpdateDto.Title!;
            task.Description = taskUpdateDto.Description!;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return MapToTaskDto(task);
        }

        public async Task<TaskDto> CompleteTaskAsync(int id, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                throw new ApplicationException("Task not found");

            if (!task.IsCompleted)
            {
                task.IsCompleted = true;
                task.EndDate = DateTime.UtcNow;
            }

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return MapToTaskDto(task);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                return false;

            _taskRepository.Delete(task);
            return await _taskRepository.SaveChangesAsync();
        }

        private TaskDto MapToTaskDto(UserTask task)
        {
            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = TimeZoneInfo.ConvertTimeFromUtc(task.StartDate, istZone),
                EndDate = task.EndDate.HasValue ?
                    TimeZoneInfo.ConvertTimeFromUtc(task.EndDate.Value, istZone) : null,
                IsCompleted = task.IsCompleted,
                TimeToComplete = task.TimeToComplete
            };
        }
    }
}