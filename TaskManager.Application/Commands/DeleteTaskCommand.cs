using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Commands
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
    }
}
