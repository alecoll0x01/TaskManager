using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.DomainEvents;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class Project : Entity
    {
        public const int MaxTasksPerProject = 20;

        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private readonly List<Task> _tasks = new();
        public IReadOnlyList<Task> Tasks => _tasks.AsReadOnly();

        // EF Core constructor
        private Project() { }

        public Project(string title, string description, Guid userId) : base()
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UserId = userId;

            AddDomainEvent(new ProjectCreatedDomainEvent(Id, title, userId));
        }

        public void UpdateDetails(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Title = title;
            Description = description;
            MarkAsModified();
        }

        public Task AddTask(string title, string description, DateTime dueDate,
                          TaskPriority priority, Guid userId)
        {
            if (_tasks.Count >= MaxTasksPerProject)
                throw new DomainException($"Project cannot have more than {MaxTasksPerProject} tasks");

            var task = new Task(title, description, dueDate, priority, Id, userId);
            _tasks.Add(task);

            AddDomainEvent(new TaskCreatedDomainEvent(task.Id, Id, title, userId));

            return task;
        }

        public void RemoveTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new DomainException("Task not found in this project");

            _tasks.Remove(task);
            AddDomainEvent(new TaskRemovedDomainEvent(taskId, Id));
        }

        public bool CanBeDeleted()
        {
            return !_tasks.Any(t => t.Status == Domain.ValueObjects.TaskStatus.Pending);
        }

        public int GetPendingTasksCount()
        {
            return _tasks.Count(t => t.Status == Domain.ValueObjects.TaskStatus.Pending);
        }
    }

}
