using BCrypt.Net;
using leave_a_note_core.Extensions;
using leave_a_note_core.Models.DTOs;
using leave_a_note_core.Services.PasswordHasher;
using leave_a_note_data.Repositories;

namespace leave_a_note_core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserViewDto>> GetAllUsersAsync()
        {
            return (await _userRepository.GetAllAsync())
                .Select(user => user.ToUserViewDto())
                .ToList();
        }

        public async Task<UserViewDto> GetUserByIdAsync(int id)
        {
            return (await _userRepository.GetAsync(id))
                .ToUserViewDto();
        }

        public async Task<UserViewDto> AddUserAsync(UserCreateDto userCreateDto)
        {
            userCreateDto.Password = _passwordHasher.HashPassword(userCreateDto.Password);
            var newUser = userCreateDto.ToUserEntity();
            await _userRepository.AddAsync(newUser);
            return newUser.ToUserViewDto();
        }

        public async Task<UserViewDto> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var userToUpdate = await _userRepository.UpdateAsync(userUpdateDto.ToUserEntity());
            return userToUpdate.ToUserViewDto();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserViewDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var userToUpdate = (await _userRepository.GetAsync(userChangePasswordDto.Id)).ToUserPasswordUpdateDto();
            
            if (!_passwordHasher.VerifyPassword(userChangePasswordDto.OldPassword, userToUpdate.Password))
            {
                throw new BcryptAuthenticationException();
            }

            userToUpdate.Password = _passwordHasher.HashPassword(userChangePasswordDto.NewPassword);

            return (await _userRepository.UpdateWithPasswordAsync(userToUpdate.ToUserEntity())).ToUserViewDto();
        }
    }
}
