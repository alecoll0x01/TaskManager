using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;

namespace TaskManagement.Domain.Tests.Entities
{
    public class TaskTests
    {
        [Fact]
        public void Task_Constructor_Should_Create_Valid_Task()
        {
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.UtcNow.AddDays(7);
            var priority = TaskPriority.High;
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var task = new TaskManager.Domain.Entities.Task(title, description, dueDate, priority, projectId, userId);

            task.Id.Should().NotBeEmpty();
            task.Title.Should().Be(title);
            task.Description.Should().Be(description);
            task.DueDate.Should().Be(dueDate);
            task.Priority.Should().Be(priority);
            task.Status.Should().Be(TaskManager.Domain.ValueObjects.TaskStatus.Pending);
            task.ProjectId.Should().Be(projectId);
            task.UserId.Should().Be(userId);
            task.Comments.Should().BeEmpty();
            task.History.Should().HaveCount(1); // Task created entry
        }

        [Fact]
        public void Task_Constructor_Should_Throw_When_Title_Is_Null()
        {
            string title = null;
            var description = "Test Description";
            var dueDate = DateTime.UtcNow.AddDays(7);
            var priority = TaskPriority.High;
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var action = () => new TaskManager.Domain.Entities.Task(title, description, dueDate, priority, projectId, userId);
            action.Should().Throw<ArgumentNullException>().WithParameterName("title");
        }

        [Fact]
        public void UpdateStatus_Should_Update_Status_And_Add_History()
        {
            var task = CreateValidTask();
            var userId = Guid.NewGuid();
            var newStatus = TaskManager.Domain.ValueObjects.TaskStatus.InProgress;

            task.UpdateStatus(newStatus, userId);

            task.Status.Should().Be(newStatus);
            task.History.Should().HaveCount(2); // Created + Status changed
            task.UpdatedAt.Should().NotBeNull();
            task.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdateStatus_Should_Not_Update_When_Same_Status()
        {
            var task = CreateValidTask();
            var userId = Guid.NewGuid();
            var originalHistoryCount = task.History.Count;

            task.UpdateStatus(TaskManager.Domain.ValueObjects.TaskStatus.Pending, userId); // Same as initial status

            task.Status.Should().Be(TaskManager.Domain.ValueObjects.TaskStatus.Pending);
            task.History.Should().HaveCount(originalHistoryCount); // No new history entry
        }

        [Fact]
        public void AddComment_Should_Add_Comment_And_History()
        {
            var task = CreateValidTask();
            var userId = Guid.NewGuid();
            var content = "Test comment";

            task.AddComment(content, userId);

            // Assert
            task.Comments.Should().HaveCount(1);
            task.Comments[0].Content.Should().Be(content);
            task.Comments[0].UserId.Should().Be(userId);
            task.History.Should().HaveCount(2); // Created + Comment added
        }

        [Fact]
        public void AddComment_Should_Throw_When_Content_Is_Empty()
        {
            var task = CreateValidTask();
            var userId = Guid.NewGuid();
            var content = "";

            var action = () => task.AddComment(content, userId);
            action.Should().Throw<ArgumentException>().WithParameterName("content");
        }

        [Fact]
        public void UpdateDetails_Should_Update_Properties_And_Add_History()
        {
            var task = CreateValidTask();
            var userId = Guid.NewGuid();
            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.UtcNow.AddDays(14);

            task.UpdateDetails(newTitle, newDescription, newDueDate, userId);

            task.Title.Should().Be(newTitle);
            task.Description.Should().Be(newDescription);
            task.DueDate.Should().Be(newDueDate);
            task.UpdatedAt.Should().NotBeNull();
            task.History.Should().HaveCount(2); // Created + Updated
        }

        [Fact]
        public void IsOverdue_Should_Return_True_When_Past_Due_And_Not_Completed()
        {
            var task = CreateValidTask();
            var pastDueDate = DateTime.UtcNow.AddDays(-1);
            task.UpdateDetails(task.Title, task.Description, pastDueDate, task.UserId);

            var isOverdue = task.IsOverdue();
            isOverdue.Should().BeTrue();
        }

        [Fact]
        public void IsOverdue_Should_Return_False_When_Completed()
        {
            var task = CreateValidTask();
            var pastDueDate = DateTime.UtcNow.AddDays(-1);
            task.UpdateDetails(task.Title, task.Description, pastDueDate, task.UserId);
            task.UpdateStatus(TaskManager.Domain.ValueObjects.TaskStatus.Completed, task.UserId);

            var isOverdue = task.IsOverdue();
            isOverdue.Should().BeFalse();
        }

        private static TaskManager.Domain.Entities.Task CreateValidTask()
        {
            return new TaskManager.Domain.Entities.Task(
                "Test Task",
                "Test Description",
                DateTime.UtcNow.AddDays(7),
                TaskPriority.Medium,
                Guid.NewGuid(),
                Guid.NewGuid());
        }
    }

}
