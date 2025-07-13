using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }

}
