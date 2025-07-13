using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.DomainEvents
{
    public class ProjectCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public Guid ProjectId { get; }
        public string Title { get; }
        public Guid UserId { get; }

        public ProjectCreatedDomainEvent(Guid projectId, string title, Guid userId)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            ProjectId = projectId;
            Title = title;
            UserId = userId;
        }
    }

}
