using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Data.Repositories;
using TaskManagementApi.Models;
using TaskManagementApi.Models.DTOs;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllForUserAsync(int userId);
        Task<TaskDto> GetByIdAsync(int id, int userId);
        Task<TaskDto> CreateAsync(TaskCreateUpdateDto taskCreateDto, int userId);
        Task<TaskDto> UpdateAsync(int id, TaskCreateUpdateDto taskUpdateDto, int userId);
        Task<TaskDto> CompleteTaskAsync(int id, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}