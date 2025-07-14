using Xunit;
using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;
using TaskManager.Domain.Exceptions;

namespace TaskManagement.Domain.Tests.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void Project_Constructor_Should_Create_Valid_Project()
        {
            var title = "Test Project";
            var description = "Test Description";
            var userId = Guid.NewGuid();

            var project = new Project(title, description, userId);

            project.Id.Should().NotBeEmpty();
            project.Title.Should().Be(title);
            project.Description.Should().Be(description);
            project.UserId.Should().Be(userId);
            project.Tasks.Should().BeEmpty();
            project.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            project.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void Project_Constructor_Should_Throw_When_Title_Is_Null()
        {
            string title = null;
            var description = "Test Description";
            var userId = Guid.NewGuid();

            var action = () => new Project(title, description, userId);
            action.Should().Throw<ArgumentNullException>().WithParameterName("title");
        }

        [Fact]
        public void AddTask_Should_Add_Task_Successfully()
        {
            var project = CreateValidProject();
            var taskTitle = "Test Task";
            var taskDescription = "Test Task Description";
            var dueDate = DateTime.UtcNow.AddDays(7);
            var priority = TaskPriority.High;
            var userId = Guid.NewGuid();

            var task = project.AddTask(taskTitle, taskDescription, dueDate, priority, userId);

            task.Should().NotBeNull();
            task.Title.Should().Be(taskTitle);
            task.ProjectId.Should().Be(project.Id);
            project.Tasks.Should().HaveCount(1);
            project.Tasks.Should().Contain(task);
            project.DomainEvents.Should().HaveCount(2); // ProjectCreated + TaskCreated
        }

        [Fact]
        public void AddTask_Should_Throw_When_Max_Tasks_Exceeded()
        {
            var project = CreateValidProject();
            var userId = Guid.NewGuid();

            for (int i = 0; i < Project.MaxTasksPerProject; i++)
            {
                project.AddTask($"Task {i}", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Low, userId);
            }
            var action = () => project.AddTask("Extra Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Low, userId);
            action.Should().Throw<DomainException>()
                .WithMessage($"Project cannot have more than {Project.MaxTasksPerProject} tasks");
        }

        [Fact]
        public void CanBeDeleted_Should_Return_True_When_No_Pending_Tasks()
        {
            var project = CreateValidProject();
            var userId = Guid.NewGuid();
            var task = project.AddTask("Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Low, userId);
            task.UpdateStatus(TaskManager.Domain.ValueObjects.TaskStatus.Completed, userId);

            var canBeDeleted = project.CanBeDeleted();
            canBeDeleted.Should().BeTrue();
        }

        [Fact]
        public void CanBeDeleted_Should_Return_False_When_Has_Pending_Tasks()
        {
            var project = CreateValidProject();
            var userId = Guid.NewGuid();
            project.AddTask("Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Low, userId);

            var canBeDeleted = project.CanBeDeleted();
            canBeDeleted.Should().BeFalse();
        }

        [Fact]
        public void RemoveTask_Should_Remove_Task_Successfully()
        {
            var project = CreateValidProject();
            var userId = Guid.NewGuid();
            var task = project.AddTask("Task", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.Low, userId);

            project.RemoveTask(task.Id);
            project.Tasks.Should().BeEmpty();
            project.DomainEvents.Should().HaveCount(3);
        }

        [Fact]
        public void RemoveTask_Should_Throw_When_Task_Not_Found()
        {
            var project = CreateValidProject();
            var nonExistentTaskId = Guid.NewGuid();

            var action = () => project.RemoveTask(nonExistentTaskId);
            action.Should().Throw<DomainException>()
                .WithMessage("Task not found in this project");
        }

        private static Project CreateValidProject()
        {
            return new Project("Test Project", "Test Description", Guid.NewGuid());
        }
    }
}