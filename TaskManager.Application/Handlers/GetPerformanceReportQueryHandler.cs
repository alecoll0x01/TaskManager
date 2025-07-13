using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Queries;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Handlers
{
    public class GetPerformanceReportQueryHandler : IRequestHandler<GetPerformanceReportQuery, PerformanceReport>
    {
        private readonly IPerformanceReportService _performanceReportService;
        private readonly IUserRepository _userRepository;

        public GetPerformanceReportQueryHandler(
            IPerformanceReportService performanceReportService,
            IUserRepository userRepository)
        {
            _performanceReportService = performanceReportService;
            _userRepository = userRepository;
        }

        public async Task<PerformanceReport> Handle(GetPerformanceReportQuery request, CancellationToken cancellationToken)
        {
            var requestingUser = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requestingUser == null)
                throw new EntityNotFoundException("User", request.RequestingUserId);

            if (!requestingUser.IsManager())
                throw new BusinessRuleViolationException("Only managers can access performance reports");

            return await _performanceReportService.GenerateUserPerformanceReportAsync(request.UserId, request.Days);
        }
    }

}
