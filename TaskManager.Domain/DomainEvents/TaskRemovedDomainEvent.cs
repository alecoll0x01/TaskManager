using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class TaskRemovedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid TaskId { get; }
        public Guid ProjectId { get; }

        public TaskRemovedDomainEvent(Guid taskId, Guid projectId)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            TaskId = taskId;
            ProjectId = projectId;
        }
    }

}
