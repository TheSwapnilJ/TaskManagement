using AutoMapper;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<TaskCreateDto, Model.Task>();
            CreateMap<Model.Task, TaskReadDto>();
            CreateMap<TaskCommentDto, TaskComment>();
            CreateMap<TaskComment, TaskCommentDto>();
        }
    }
}
