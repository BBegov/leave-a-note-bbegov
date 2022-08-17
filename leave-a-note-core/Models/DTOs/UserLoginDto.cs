using System.ComponentModel.DataAnnotations;
using leave_a_note_data.Entities;

namespace leave_a_note_core.Models.DTOs;

public class UserLoginDto
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public UserRole Role { get; set; }
}
