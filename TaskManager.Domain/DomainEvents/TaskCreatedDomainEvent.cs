using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class TaskCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid TaskId { get; }
        public Guid ProjectId { get; }
        public string Title { get; }
        public Guid UserId { get; }

        public TaskCreatedDomainEvent(Guid taskId, Guid projectId, string title, Guid userId)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            TaskId = taskId;
            ProjectId = projectId;
            Title = title;
            UserId = userId;
        }
    }

}
