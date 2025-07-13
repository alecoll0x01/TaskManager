using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Entities.Task> GetByIdAsync(Guid id);
        Task<IEnumerable<Entities.Task>> GetByProjectIdAsync(Guid projectId);
        Task<Entities.Task> CreateAsync(Entities.Task task);
        Task<Entities.Task> UpdateAsync(Entities.Task task);
        System.Threading.Tasks.Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetCompletedTasksCountByUserAsync(Guid userId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
    }

}
