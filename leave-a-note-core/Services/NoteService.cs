using leave_a_note_core.Extensions;
using leave_a_note_core.Models.DTOs;
using leave_a_note_data.Entities;
using leave_a_note_data.Repositories;

namespace leave_a_note_core.Services;

public class NoteService : INoteService
{
    private readonly IRepository<Note> _noteRepository;
    private readonly IUserRepository _userRepository;

    public NoteService(IRepository<Note> noteRepository, IUserRepository userRepository)
    {
        _noteRepository = noteRepository;
        _userRepository = userRepository;
    }

    public async Task<List<NoteViewDto>> GetAllNotesAsync()
    {
        return (await _noteRepository.GetAllAsync())
            .ToNoteListViewDto();
    }

    public async Task<NoteViewDto> GetNoteByIdAsync(int id)
    {
        return (await _noteRepository.GetAsync(id))
            .ToNoteViewDto();
    }

    public async Task<NoteViewDto> AddNoteAsync(NoteCreateDto noteCreateDto)
    {
        var newNote = noteCreateDto.ToNoteEntity();
        await _noteRepository.AddAsync(newNote);
        return newNote.ToNoteViewDto();
    }

    public async Task<NoteViewDto> UpdateNoteAsync(NoteUpdateDto noteUpdateDto)
    {
        var noteToUpdate = noteUpdateDto.ToNoteEntity();
        await _noteRepository.UpdateAsync(noteToUpdate);
        return noteToUpdate.ToNoteViewDto();
    }

    public async Task DeleteNoteAsync(int id)
    {
        await _noteRepository.DeleteAsync(id);
    }
}
