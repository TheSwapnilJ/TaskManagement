using Xunit;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Dtos;
using System.Collections.Generic;

namespace TaskManagementSystem.Tests.Dtos
{
    public class DtoValidationTests
    {
        // Method to validate the DTO objects using Data Annotations
        private IList<ValidationResult> Validate(object dto)
        {
            var context = new ValidationContext(dto, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, context, results, true);
            return results;
        }

        // Test case to validate that an invalid email address triggers a validation error
        [Fact]
        public void UserRegisterDto_InvalidEmail_ReturnsValidationError()
        {
            // Arrange: Create a UserRegisterDto with an invalid email address
            var dto = new UserRegisterDto { Username = "u", Password = "p" };

            // Act: Validate the DTO
            var results = Validate(dto);

            // Assert: Ensure that there is a validation error for the "Email" field
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        // Test case to validate that a missing title in TaskCreateDto triggers a validation error
        [Fact]
        public void TaskCreateDto_MissingTitle_ReturnsValidationError()
        {
            // Arrange: Create a TaskCreateDto with a missing title (assuming Title is required)
            var dto = new TaskCreateDto { Description = "desc" };

            // Act: Validate the DTO
            var results = Validate(dto);

            // Assert: Ensure that there is a validation error for the "Title" field
            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
        }

        // Additional test case to validate email format in UserRegisterDto (optional)
        [Fact]
        public void UserRegisterDto_InvalidEmailFormat_ReturnsValidationError()
        {
            // Arrange: Create a UserRegisterDto with an invalid email format
            var dto = new UserRegisterDto { Username = "u", Password = "p" };

            // Act: Validate the DTO
            var results = Validate(dto);

            // Assert: Ensure that there is a validation error for the "Email" field
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        // Additional test case to validate password length in UserRegisterDto (optional)
        [Fact]
        public void UserRegisterDto_InvalidPasswordLength_ReturnsValidationError()
        {
            // Arrange: Create a UserRegisterDto with a short password (assuming min length is 6)
            var dto = new UserRegisterDto { Username = "u", Password = "p" };

            // Act: Validate the DTO
            var results = Validate(dto);

            // Assert: Ensure that there is a validation error for the "Password" field if it's too short
            Assert.Contains(results, r => r.MemberNames.Contains("Password"));
        }
    }
}
