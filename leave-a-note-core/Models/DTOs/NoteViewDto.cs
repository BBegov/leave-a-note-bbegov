namespace leave_a_note_core.Models.DTOs;

public class NoteViewDto
{
    public int Id { get; set; }
    public string NoteText { get; set; }
    public DateTime PublishDate { get; set; }
    public int UserId { get; set; }
}
