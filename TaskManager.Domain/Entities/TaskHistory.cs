using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class TaskHistory : Entity
    {
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        private TaskHistory() { }
        public TaskHistory(string description, Guid userId) : base()
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UserId = userId;
        }
    }

}
