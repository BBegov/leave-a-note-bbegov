using System.ComponentModel.DataAnnotations;

namespace leave_a_note_core.Models.DTOs;

public class NoteCreateDto
{
    [Required]
    [StringLength(255, ErrorMessage = "Note text cannot be more than 255 characters long")]
    public string NoteText { get; set; }

    [Required]
    public int UserId { get; set; }
}
