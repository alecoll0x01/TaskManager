using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Commands
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }

}
