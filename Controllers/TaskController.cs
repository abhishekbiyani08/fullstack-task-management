using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetTaskHistory([FromQuery] int userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            Console.WriteLine($"userId: {userId}, startDate: {startDate}, endDate: {endDate}");

            var query = _context.Tasks.AsQueryable();

            query = query.Where(t => t.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= endDate.Value || t.EndDate == null);
            }

            var tasks = await query.ToListAsync();

            return Ok(tasks);
        }
    }
}