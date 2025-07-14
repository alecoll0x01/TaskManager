using Xunit;
using FluentAssertions;
using Moq;
using TaskManager.Application.Commands;
using TaskManager.Application.Validators;

namespace TaskManager.Application.Tests.Validators
{
    public class CreateProjectCommandValidatorTests
    {
        private readonly CreateProjectCommandValidator _validator;

        public CreateProjectCommandValidatorTests()
        {
            _validator = new CreateProjectCommandValidator();
        }

        [Fact]
        public void Validate_Should_Pass_With_Valid_Command()
        {
            var command = new CreateProjectCommand
            {
                Title = "Valid Title",
                Description = "Valid Description",
                UserId = Guid.NewGuid()
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_Should_Fail_When_Title_Is_Empty()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                Title = "",
                Description = "Valid Description",
                UserId = Guid.NewGuid()
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "Title is required");
        }

        [Fact]
        public void Validate_Should_Fail_When_Title_Exceeds_Max_Length()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                Title = new string('a', 201), // 201 characters
                Description = "Valid Description",
                UserId = Guid.NewGuid()
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "Title must not exceed 200 characters");
        }

        [Fact]
        public void Validate_Should_Fail_When_UserId_Is_Empty()
        {
            var command = new CreateProjectCommand
            {
                Title = "Valid Title",
                Description = "Valid Description",
                UserId = Guid.Empty
            };
            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId" && e.ErrorMessage == "UserId is required");
        }
    }

}
