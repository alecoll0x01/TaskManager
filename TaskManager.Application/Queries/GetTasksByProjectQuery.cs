using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Queries
{
    public class GetTasksByProjectQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }

}
