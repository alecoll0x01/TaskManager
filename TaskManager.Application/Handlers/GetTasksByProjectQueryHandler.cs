using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Queries;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Handlers
{
    public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public GetTasksByProjectQueryHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new EntityNotFoundException("User", request.UserId);

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new EntityNotFoundException("Project", request.ProjectId);

            if (project.UserId != request.UserId)
                throw new BusinessRuleViolationException("User can only view tasks from their own projects");

            var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                ProjectId = t.ProjectId,
                UserId = t.UserId,
                IsOverdue = t.IsOverdue(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Comments = t.Comments.Select(c => new TaskCommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    CreatedAt = c.CreatedAt
                }).ToList(),
                History = t.History.Select(h => new TaskHistoryDto
                {
                    Id = h.Id,
                    Description = h.Description,
                    UserId = h.UserId,
                    CreatedAt = h.CreatedAt
                }).ToList()
            });
        }
    }
}
