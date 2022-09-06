using leave_a_note_core.Models.Authentication.Requests;
using leave_a_note_core.Models.Authentication.Responses;
using leave_a_note_core.Services;
using leave_a_note_core.Services.Authenticators;
using leave_a_note_core.Services.PasswordHasher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace leave_a_note_core.Controllers;

[AllowAnonymous]
[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Authenticator _authenticator;

    public AuthenticationController(IUserService userService, IPasswordHasher passwordHasher, Authenticator authenticator)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _authenticator = authenticator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }

        var user = await _userService.GetUserLoginDtoByUsername(loginRequest.Username);

        if (user == null)
        {
            return Unauthorized("User does not exist.");
        }

        var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

        if (!isCorrectPassword)
        {
            return Unauthorized("Incorrect password.");
        }

        var response = _authenticator.Authenticate(user);

        return Ok(response);
    }
}
