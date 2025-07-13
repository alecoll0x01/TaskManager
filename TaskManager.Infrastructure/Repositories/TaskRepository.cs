using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Task> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.History)
                    .ThenInclude(h => h.User)
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks
                .Include(t => t.Comments)
                .Include(t => t.History)
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Task> CreateAsync(Domain.Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
            return task;
        }

        public async Task<Domain.Entities.Task> UpdateAsync(Domain.Entities.Task task)
        {
            _context.Tasks.Update(task);
            return await System.Threading.Tasks.Task.FromResult(task);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetCompletedTasksCountByUserAsync(Guid userId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId &&
                           t.Status == Domain.ValueObjects.TaskStatus.Completed &&
                           t.UpdatedAt >= fromDate &&
                           t.UpdatedAt <= toDate)
                .CountAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Tasks
                .Where(t => t.DueDate < now && t.Status != Domain.ValueObjects.TaskStatus.Completed)
                .ToListAsync();
        }
    }

}
