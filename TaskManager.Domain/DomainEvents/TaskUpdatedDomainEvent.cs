using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class TaskUpdatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid TaskId { get; }
        public Guid ProjectId { get; }
        public Guid UserId { get; }

        public TaskUpdatedDomainEvent(Guid taskId, Guid projectId, Guid userId)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            TaskId = taskId;
            ProjectId = projectId;
            UserId = userId;
        }
    }

}
