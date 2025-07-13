using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Services
{
    public interface IPerformanceReportService
    {
        Task<double> GetAverageCompletedTasksPerUserAsync(int days = 30);
        Task<PerformanceReport> GenerateUserPerformanceReportAsync(Guid userId, int days = 30);
    }

    public class PerformanceReport
    {
        public Guid UserId { get; set; }
        public int CompletedTasks { get; set; }
        public double AverageTasksPerDay { get; set; }
        public int OverdueTasks { get; set; }
        public DateTime ReportGeneratedAt { get; set; }
    }
}
