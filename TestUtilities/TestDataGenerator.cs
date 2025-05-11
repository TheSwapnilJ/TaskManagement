using TaskManagementSystem.Model;
using System.Collections.Generic;
using TaskManagementSystem.Enums;

namespace TaskManagementSystem.Tests.TestUtilities
{
    public static class TestDataGenerator
    {
        // Method to create a single Task
        public static Model.Task CreateTask(int id = 1)
        {
            return new Model.Task
            {
                Id = id,
                Title = "Test Task",
                Description = "Desc",
                Status = "Open",  // Directly assigning string value
                AssignedUserId = 1        // Assuming this is an int
            };
        }

        // Method to create a single TaskComment
        public static TaskComment CreateComment(int id = 1, int taskId = 1, string commentText = "Test Comment", int userId = 1)
        {
            return new TaskComment
            {
                Id = id,
                TaskId = taskId,
                Text = commentText,  // Use 'Text' instead of 'Comment' to match the property
                UserId = userId
            };
        }



        // Method to create a single User
        public static User CreateUser(int id = 1)
        {
            return new User
            {
                Id = id,
                Username = $"user{id}", // Dynamically generate usernames like user1, user2, etc.
                // Dynamically generate email addresses
                Role = RoleEnum.User.ToString()  // Assuming RoleEnum is an enum and User is a valid role
            };
        }

        // Method to create a list of Task objects
        public static IEnumerable<Model.Task> CreateTasks(int count = 3)
        {
            var list = new List<Model.Task>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(CreateTask(i));
            }
            return list.Count > 0 ? list : new List<Model.Task> { CreateTask() }; // Ensure list is never empty
        }

        // Method to create a list of TaskComment objects
        public static IEnumerable<TaskComment> CreateComments(int count = 3)
        {
            var list = new List<TaskComment>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(CreateComment(i));
            }
            return list.Count > 0 ? list : new List<TaskComment> { CreateComment() }; // Ensure list is never empty
        }

        // Method to create a list of User objects
        public static IEnumerable<User> CreateUsers(int count = 3)
        {
            var list = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(CreateUser(i));
            }
            return list.Count > 0 ? list : new List<User> { CreateUser() }; // Ensure list is never empty
        }
    }
}
