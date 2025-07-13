using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.DomainEvents;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class Task : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskPriority Priority { get; private set; }
        public ValueObjects.TaskStatus Status { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private readonly List<TaskComment> _comments = new();
        public IReadOnlyList<TaskComment> Comments => _comments.AsReadOnly();

        private readonly List<TaskHistory> _history = new();
        public IReadOnlyList<TaskHistory> History => _history.AsReadOnly();

        // EF Core constructor
        private Task() { }

        public Task(string title, string description, DateTime dueDate,
                   TaskPriority priority, Guid projectId, Guid userId) : base()
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            DueDate = dueDate;
            Priority = priority;
            Status = ValueObjects.TaskStatus.Pending;
            ProjectId = projectId;
            UserId = userId;

            AddHistoryEntry("Task created", userId);
        }

        public void UpdateDetails(string title, string description, DateTime dueDate, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            var changes = new List<string>();

            if (Title != title)
            {
                Title = title;
                changes.Add($"Title changed to '{title}'");
            }

            if (Description != description)
            {
                Description = description;
                changes.Add("Description updated");
            }

            if (DueDate != dueDate)
            {
                DueDate = dueDate;
                changes.Add($"Due date changed to {dueDate:yyyy-MM-dd}");
            }

            if (changes.Any())
            {
                MarkAsModified();
                AddHistoryEntry(string.Join(", ", changes), userId);
                AddDomainEvent(new TaskUpdatedDomainEvent(Id, ProjectId, userId));
            }
        }

        public void UpdateStatus(ValueObjects.TaskStatus newStatus, Guid userId)
        {
            if (Status == newStatus) return;

            var oldStatus = Status;
            Status = newStatus;
            MarkAsModified();

            AddHistoryEntry($"Status changed from {oldStatus} to {newStatus}", userId);
            AddDomainEvent(new TaskStatusChangedDomainEvent(Id, ProjectId, oldStatus, newStatus, userId));
        }

        public void AddComment(string content, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content cannot be empty", nameof(content));

            var comment = new TaskComment(content, userId);
            _comments.Add(comment);

            AddHistoryEntry($"Comment added: {content}", userId);
            AddDomainEvent(new TaskCommentAddedDomainEvent(Id, ProjectId, userId, content));
        }

        private void AddHistoryEntry(string description, Guid userId)
        {
            var historyEntry = new TaskHistory(description, userId);
            _history.Add(historyEntry);
        }

        public bool IsOverdue()
        {
            return DueDate < DateTime.UtcNow && Status != ValueObjects.TaskStatus.Completed;
        }
    }
}
