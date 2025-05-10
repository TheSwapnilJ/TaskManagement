# Task Management System

A Task Management System built using ASP.NET Core Web API. This project includes user registration and login with role-based authorization, task management features (CRUD), and task commenting capabilities.

## 📁 Project Structure

```
TaskManagementSystem/
├── Connected Services/
├── Dependencies/
├── Properties/
├── Controllers/
│   ├── AuthController.cs
│   ├── TaskCommentController.cs
│   ├── TaskController.cs
│   └── UserController.cs
├── Data/
│   └── TaskDbContext.cs
├── Dtos/
│   ├── LoginDto.cs
│   ├── TaskCommentDto.cs
│   ├── TaskCreateDto.cs
│   ├── TaskReadDto.cs
│   ├── UserDto.cs
│   └── UserRegisterDto.cs
├── Enum/
│   └── RoleEnum.cs
├── Mappings/
│   └── MappingProfile.cs
├── Migrations/
├── Model/
│   ├── Task.cs
│   ├── TaskComment.cs
│   └── User.cs
├── Service/
│   ├── ITaskCommentService.cs
│   ├── ITaskService.cs
│   ├── IUserService.cs
│   ├── TaskCommentService.cs
│   ├── TaskService.cs
│   └── UserService.cs
├── appsettings.json
└── Program.cs
```

## ✅ Features

- 🔐 User Registration & Login
- 👤 Role-Based Authorization (Admin, User)
- 📝 Task Creation, Assignment, and Management
- 💬 Commenting on Tasks
- 🧩 Layered Architecture: Controllers, Services, DTOs, and Models
- 🛠️ AutoMapper for DTO-Mapping
- 📦 Entity Framework Core with Migrations
- 🔐 JWT Authentication

## 🔧 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- JWT Authentication
- SQL Server

## 🏁 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/TheSwapnilJ/TaskManagement.git
cd TaskManagementSystem
```

### 2. Update `appsettings.json`

Make sure to update your database connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TaskDB;Trusted_Connection=True;"
}
```

### 3. Run Migrations & Update Database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Requires `dotnet-ef` tool:  
> `dotnet tool install --global dotnet-ef`

### 4. Run the Application

```bash
dotnet run
```

The API will be available at: `https://localhost:5001` or `http://localhost:5000`

## 📂 API Endpoints

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### User
- `GET /api/user`
- `GET /api/user/{id}`

### Task
- `GET /api/task`
- `POST /api/task`
- `PUT /api/task/{id}`
- `DELETE /api/task/{id}`

### Task Comment
- `GET /api/taskcomment/{taskId}`
- `POST /api/taskcomment`

## 👤 Roles

Defined in `Enum/RoleEnum.cs`:

```csharp
public enum RoleEnum
{
    Admin,
    User
}
```

## 🤝 Contributions

Pull requests are welcome. For major changes, please open an issue first.

---

## 📄 License

[MIT](LICENSE)
