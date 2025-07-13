using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Queries
{
    public class GetPerformanceReportQuery : IRequest<PerformanceReport>
    {
        public Guid UserId { get; set; }
        public Guid RequestingUserId { get; set; }
        public int Days { get; set; } = 30;
    }
}
