using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Domain.ValueObjects.TaskStatus Status { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public bool IsOverdue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<TaskCommentDto> Comments { get; set; } = new();
        public List<TaskHistoryDto> History { get; set; } = new();
    }

}
