using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllForUserAsync(int userId);
        Task<UserTask> GetByIdAsync(int id, int userId);
        Task AddAsync(UserTask task);
        void Update(UserTask task);
        void Delete(UserTask task);
        Task<bool> SaveChangesAsync();
    }
}