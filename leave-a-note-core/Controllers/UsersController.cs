using leave_a_note_core.Models.DTOs;
using leave_a_note_core.Services;
using leave_a_note_core.Services.PasswordHasher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace leave_a_note_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewDto>>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public async Task<ActionResult<UserViewDto>> GetUser(int id)
        {
            try
            {
                return await _userService.GetUserByIdAsync(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No user with ID:{id} found.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserViewDto>> AddUser(UserCreateDto newUser)
        {
            newUser.Password = _passwordHasher.HashPassword(newUser.Password);

            try
            {
                var createdUser = await _userService.AddUserAsync(newUser);
                return CreatedAtRoute("GetUser", new {id = createdUser.Id}, createdUser);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserViewDto>> UpdateUser(UserUpdateDto updatedUser, int id)
        {
            updatedUser.Id = id;
            
            try
            {
                return await _userService.UpdateUserAsync(updatedUser);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No user with ID:{id} found.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound($"No user with ID:{id} found.");
            }
        }
    }
}
