using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Services
{
    public class PerformanceReportService : IPerformanceReportService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public PerformanceReportService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<double> GetAverageCompletedTasksPerUserAsync(int days = 30)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);
            var toDate = DateTime.UtcNow;
            var completedTasks = await _taskRepository.GetCompletedTasksCountByUserAsync(Guid.Empty, fromDate, toDate);

            return completedTasks / (double)days;
        }

        public async Task<PerformanceReport> GenerateUserPerformanceReportAsync(Guid userId, int days = 30)
        {
            if (!await _userRepository.ExistsAsync(userId))
                throw new ArgumentException("User not found", nameof(userId));

            var fromDate = DateTime.UtcNow.AddDays(-days);
            var toDate = DateTime.UtcNow;

            var completedTasks = await _taskRepository.GetCompletedTasksCountByUserAsync(userId, fromDate, toDate);
            var overdueTasks = (await _taskRepository.GetOverdueTasksAsync()).Count();

            return new PerformanceReport
            {
                UserId = userId,
                CompletedTasks = completedTasks,
                AverageTasksPerDay = completedTasks / (double)days,
                OverdueTasks = overdueTasks,
                ReportGeneratedAt = DateTime.UtcNow
            };
        }
    }

}
