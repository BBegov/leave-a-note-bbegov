﻿using System.ComponentModel.DataAnnotations;

namespace leave_a_note_core.Models.DTOs;

public class UserUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 32 characters long")]
    public string UserName { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 32 characters long")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 32 characters long")]
    public string LastName { get; set; }
}
