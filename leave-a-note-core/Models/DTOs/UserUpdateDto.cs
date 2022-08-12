using System.ComponentModel.DataAnnotations;

namespace leave_a_note_core.Models.DTOs;

public class UserUpdateDto
{
    [Required]
    [StringLength(32, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 32 characters long")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 32 characters long")]
    public string LastName { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 32 characters long")]
    public string Password { get; set; }
}
