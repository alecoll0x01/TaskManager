using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class TaskCommentAddedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid TaskId { get; }
        public Guid ProjectId { get; }
        public Guid UserId { get; }
        public string Content { get; }
        public TaskCommentAddedDomainEvent(Guid taskId, Guid projectId, Guid userId, string content)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            TaskId = taskId;
            ProjectId = projectId;
            UserId = userId;
            Content = content;
        }
    }

}
