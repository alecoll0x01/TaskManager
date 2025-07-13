using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commands;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new EntityNotFoundException("User", request.UserId);

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new EntityNotFoundException("Project", request.ProjectId);

            if (project.UserId != request.UserId)
                throw new BusinessRuleViolationException("User can only add tasks to their own projects");

            var task = project.AddTask(
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority,
                request.UserId);

            await _projectRepository.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                ProjectId = task.ProjectId,
                UserId = task.UserId,
                IsOverdue = task.IsOverdue(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }

}
