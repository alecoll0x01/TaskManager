using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class TaskStatusChangedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid TaskId { get; }
        public Guid ProjectId { get; }
        public Domain.ValueObjects.TaskStatus OldStatus { get; }
        public Domain.ValueObjects.TaskStatus NewStatus { get; }
        public Guid UserId { get; }

        public TaskStatusChangedDomainEvent(Guid taskId, Guid projectId,
                                          Domain.ValueObjects.TaskStatus oldStatus, Domain.ValueObjects.TaskStatus newStatus, Guid userId)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            TaskId = taskId;
            ProjectId = projectId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            UserId = userId;
        }
    }

}
