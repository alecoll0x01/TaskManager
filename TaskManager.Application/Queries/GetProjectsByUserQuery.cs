using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Queries
{
    public class GetProjectsByUserQuery : IRequest<IEnumerable<ProjectDto>>
    {
        public Guid UserId { get; set; }
    }

}
