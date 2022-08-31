using System.ComponentModel.DataAnnotations;

namespace leave_a_note_core.Models.Authentication.Requests;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
