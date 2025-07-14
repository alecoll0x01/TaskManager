using FluentAssertions;
using Moq;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Tests.Handlers
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateTaskCommandHandler(
                _projectRepositoryMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Throw_When_User_Not_Found()
        {
            var userId = Guid.NewGuid();
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                UserId = userId
            };

            _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(false);

            var action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"User with ID {userId} was not found");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Throw_When_Project_Not_Found()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                ProjectId = projectId,
                UserId = userId
            };

            _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(true);

            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
                .ReturnsAsync((Project)null);

            var action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Project with ID {projectId} was not found");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Throw_When_User_Not_Project_Owner()
        {
            var userId = Guid.NewGuid();
            var projectOwnerId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var project = new Project("Test Project", "Test Description", projectOwnerId);

            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                ProjectId = projectId,
                UserId = userId
            };

            _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(true);

            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            var action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<BusinessRuleViolationException>()
                .WithMessage("User can only add tasks to their own projects");
        }
    }

}
