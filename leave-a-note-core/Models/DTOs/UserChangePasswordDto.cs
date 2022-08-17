using System.ComponentModel.DataAnnotations;

namespace leave_a_note_core.Models.DTOs;

public class UserChangePasswordDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 32 characters long")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 32 characters long")]
    public string NewPassword { get; set; }
}
