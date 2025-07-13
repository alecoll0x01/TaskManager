using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Queries;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Handlers
{
    public class GetProjectsByUserQueryHandler : IRequestHandler<GetProjectsByUserQuery, IEnumerable<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public GetProjectsByUserQueryHandler(
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new EntityNotFoundException("User", request.UserId);

            var projects = await _projectRepository.GetByUserIdAsync(request.UserId);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                UserId = p.UserId,
                TasksCount = p.Tasks.Count,
                PendingTasksCount = p.GetPendingTasksCount(),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }
    }

}
