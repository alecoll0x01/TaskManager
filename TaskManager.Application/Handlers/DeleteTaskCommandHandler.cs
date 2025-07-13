using MediatR;
using TaskManager.Application.Commands;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new EntityNotFoundException("User", request.UserId);

            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new EntityNotFoundException("Task", request.TaskId);

            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            if (project == null)
                throw new EntityNotFoundException("Project", task.ProjectId);

            if (project.UserId != request.UserId)
                throw new BusinessRuleViolationException("User can only delete tasks from their own projects");

            project.RemoveTask(request.TaskId);

            await _projectRepository.UpdateAsync(project);
            await _taskRepository.DeleteAsync(request.TaskId);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
