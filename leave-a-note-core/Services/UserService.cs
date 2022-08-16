using leave_a_note_core.Extensions;
using leave_a_note_core.Models.DTOs;
using leave_a_note_data.Entities;
using leave_a_note_data.Repositories;

namespace leave_a_note_core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
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
    }
}
