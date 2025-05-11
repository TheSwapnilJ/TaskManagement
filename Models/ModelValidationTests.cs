using Xunit;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Model;
using TaskManagementSystem.Enums;
using System.Collections.Generic;

namespace TaskManagementSystem.Tests.Models
{
    public class ModelValidationTests
    {
        private IList<ValidationResult> Validate(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void Task_InvalidWithoutTitle_ReturnsValidationError()
        {
            // Arrange: Create a Task instance without a Title (required field).
            var model = new Model.Task { Description = "Sample description" }; // Missing Title

            // Act: Validate the Task model.
            var results = Validate(model);

            // Assert: Check that the validation error contains the "Title" member name.
            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
        }

        [Fact]
        public void User_InvalidWithoutUsername_ReturnsValidationError()
        {
            // Arrange: Create a User instance without a Username (required field).
            var model = new User {  Role = RoleEnum.User.ToString() }; // Missing Username

            // Act: Validate the User model.
            var results = Validate(model);

            // Assert: Check that the validation error contains the "Username" member name.
            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }

        [Fact]
        public void User_InvalidWithoutEmail_ReturnsValidationError()
        {
            // Arrange: Create a User instance without an Email (required field).
            var model = new User { Username = "testuser", Role = RoleEnum.User.ToString() }; // Missing Email

            // Act: Validate the User model.
            var results = Validate(model);

            // Assert: Check that the validation error contains the "Email" member name.
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void Task_InvalidWithTooLongTitle_ReturnsValidationError()
        {
            // Arrange: Create a Task instance with a Title that exceeds the maximum length (assuming max length 100).
            var model = new Model.Task { Title = new string('a', 101), Description = "Valid description" };

            // Act: Validate the Task model.
            var results = Validate(model);

            // Assert: Check that the validation error contains the "Title" member name.
            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
        }
    }
}
