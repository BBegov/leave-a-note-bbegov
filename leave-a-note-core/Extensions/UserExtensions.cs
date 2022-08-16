using leave_a_note_core.Models.DTOs;
using leave_a_note_data.Entities;

namespace leave_a_note_core.Extensions
{
    public static class UserExtensions
    {
        public static UserViewDto ToUserViewDto(this User user)
        {
            return new UserViewDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString()
            };
        }

        public static List<UserViewDto> ToUserListViewDto(this List<User> users)
        {
            return users.Select(ToUserViewDto).ToList();
        }

        public static User ToUserEntity(this UserCreateDto userCreateDto)
        {
            var userRole = Enum.Parse<UserRole>(userCreateDto.Role);
            return new User
            {
                UserName = userCreateDto.UserName,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Role = userRole,
                PasswordHash = userCreateDto.Password
            };
        }

        public static User ToUserEntity(this UserUpdateDto userUpdateDto)
        {
            return new User
            {
                Id = userUpdateDto.Id,
                UserName = userUpdateDto.UserName,
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName
            };
        }
    }
}
