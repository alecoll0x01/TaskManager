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
    public class AddTaskCommentCommandHandler : IRequestHandler<AddTaskCommentCommand, TaskCommentDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTaskCommentCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskCommentDto> Handle(AddTaskCommentCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new EntityNotFoundException("User", request.UserId);

            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new EntityNotFoundException("Task", request.TaskId);

            task.AddComment(request.Content, request.UserId);

            await _taskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var lastComment = task.Comments[task.Comments.Count - 1];

            return new TaskCommentDto
            {
                Id = lastComment.Id,
                Content = lastComment.Content,
                UserId = lastComment.UserId,
                CreatedAt = lastComment.CreatedAt
            };
        }
    }

}
