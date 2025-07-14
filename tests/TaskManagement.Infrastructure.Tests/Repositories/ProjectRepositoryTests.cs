using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure.Tests.Repositories
{
    public class ProjectRepositoryTests : IDisposable
    {
        private readonly TaskManagementDbContext _context;
        private readonly ProjectRepository _repository;
        private readonly Mock<IMediator> _mediatorMock;

        public ProjectRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mediatorMock = new Mock<IMediator>();
            _context = new TaskManagementDbContext(options, _mediatorMock.Object);
            _repository = new ProjectRepository(_context);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateAsync_Should_Add_Project_To_Database()
        {
            // Arrange
            var project = new Project("Test Project", "Test Description", Guid.NewGuid());

            // Act
            var result = await _repository.CreateAsync(project);
            await _context.SaveChangesAsync();

            // Assert
            result.Should().Be(project);

            var savedProject = await _context.Projects.FindAsync(project.Id);
            savedProject.Should().NotBeNull();
            savedProject.Title.Should().Be("Test Project");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUserIdAsync_Should_Return_User_Projects()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var project1 = new Project("Project 1", "Description 1", userId);
            var project2 = new Project("Project 2", "Description 2", userId);
            var project3 = new Project("Project 3", "Description 3", otherUserId);

            await _repository.CreateAsync(project1);
            await _repository.CreateAsync(project2);
            await _repository.CreateAsync(project3);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByUserIdAsync(userId);

            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Id == project1.Id);
            result.Should().Contain(p => p.Id == project2.Id);
            result.Should().NotContain(p => p.Id == project3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task ExistsAsync_Should_Return_True_When_Project_Exists()
        {
            var project = new Project("Test Project", "Test Description", Guid.NewGuid());
            await _repository.CreateAsync(project);
            await _context.SaveChangesAsync();

            var exists = await _repository.ExistsAsync(project.Id);
            exists.Should().BeTrue();
        }

        [Fact]
        public async System.Threading.Tasks.Task ExistsAsync_Should_Return_False_When_Project_Does_Not_Exist()
        {
            var nonExistentId = Guid.NewGuid();
            var exists = await _repository.ExistsAsync(nonExistentId);
            exists.Should().BeFalse();
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteAsync_Should_Remove_Project_From_Database()
        {
            var project = new Project("Test Project", "Test Description", Guid.NewGuid());
            await _repository.CreateAsync(project);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(project.Id);
            await _context.SaveChangesAsync();

            var deletedProject = await _context.Projects.FindAsync(project.Id);
            deletedProject.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}