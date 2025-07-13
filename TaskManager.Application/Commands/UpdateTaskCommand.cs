using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Commands
{
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Domain.ValueObjects.TaskStatus? Status { get; set; }
        public Guid UserId { get; set; }
    }
}
