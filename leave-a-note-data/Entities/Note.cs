using System.ComponentModel.DataAnnotations;

namespace leave_a_note_data.Entities;

public class Note
{
    public int Id { get; set; }

    [Required]
    public string NoteText { get; set; }

    [Required]
    public DateTime PublishDate { get; set; }

    [Required]
    public User User { get; set; }
}
