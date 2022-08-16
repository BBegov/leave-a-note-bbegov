﻿using leave_a_note_core.Models.DTOs;

namespace leave_a_note_core.Services
{
    public interface IUserService
    {
        Task<List<UserViewDto>> GetAllUsersAsync();
        Task<UserViewDto> GetUserByIdAsync(int id);
        Task<UserViewDto> AddUserAsync(UserCreateDto userCreateDto);
        Task<UserViewDto> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(int id);
    }
}
