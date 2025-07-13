using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);
        Task<Project> CreateAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        System.Threading.Tasks.Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }

}
