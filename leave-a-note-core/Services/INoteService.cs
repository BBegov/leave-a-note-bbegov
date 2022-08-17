using leave_a_note_core.Models.DTOs;

namespace leave_a_note_core.Services;

public interface INoteService
{
    Task<List<NoteViewDto>> GetAllNotesAsync();
    Task<NoteViewDto> GetNoteByIdAsync(int id);
    Task<NoteViewDto> AddNoteAsync(NoteCreateDto noteCreateDto);
    Task<NoteViewDto> UpdateNoteAsync(NoteUpdateDto noteUpdateDto);
    Task DeleteNoteAsync(int id);
}
