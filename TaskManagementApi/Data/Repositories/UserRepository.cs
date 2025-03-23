using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;
using System.Threading.Tasks;
using TaskManagementApi.Data;

namespace TaskManagementApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Username!.ToLower() == username.ToLower());
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username!.ToLower() == username.ToLower()))
                return true;

            return false;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
