using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Commands
{
    public class AddTaskCommentCommand : IRequest<TaskCommentDto>
    {
        public Guid TaskId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }

}
