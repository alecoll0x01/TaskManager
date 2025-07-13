using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class TaskComment : Entity
    {
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }

        // EF Core constructor
        private TaskComment() { }

        public TaskComment(string content, Guid userId) : base()
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            UserId = userId;
        }

    }
}
