using Xunit;
using FluentAssertions;
using Moq;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Handlers;
using TaskManager.Application.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tests.Handlers
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateProjectCommandHandler(
                _projectRepositoryMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Create_Project_Successfully()
        {
            var userId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                Description = "Test Description",
                UserId = userId
            };

            _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(true);

            _projectRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Project>()))
                .ReturnsAsync((Project p) => p);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Title.Should().Be(command.Title);
            result.Description.Should().Be(command.Description);
            result.UserId.Should().Be(command.UserId);
            result.TasksCount.Should().Be(0);
            result.PendingTasksCount.Should().Be(0);

            _userRepositoryMock.Verify(x => x.ExistsAsync(userId), Times.Once);
            _projectRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Project>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Throw_When_User_Not_Found()
        {
            var userId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                Description = "Test Description",
                UserId = userId
            };

            _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(false);

            var action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"User with ID {userId} was not found");

            _projectRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Project>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }

}