using leave_a_note_core.Models.DTOs;
using leave_a_note_data.Entities;

namespace leave_a_note_core.Extensions;

public static class NoteExtensions
{
    public static NoteViewDto ToNoteViewDto(this Note note)
    {
        return new NoteViewDto
        {
            Id = note.Id,
            NoteText = note.NoteText,
            PublishDate = note.PublishDate,
            UserId = note.UserId
        };
    }

    public static List<NoteViewDto> ToNoteListViewDto(this List<Note> notes)
    {
        return notes.Select(ToNoteViewDto).ToList();
    }

    public static Note ToNoteEntity(this NoteCreateDto noteCreateDto)
    {
        return new Note
        {
            NoteText = noteCreateDto.NoteText,
            PublishDate = DateTime.Now,
            UserId = noteCreateDto.UserId
        };
    }

    public static Note ToNoteEntity(this NoteUpdateDto noteUpdateDto)
    {
        return new Note
        {
            Id = noteUpdateDto.Id,
            NoteText = noteUpdateDto.NoteText,
            PublishDate = DateTime.Now,
            UserId = noteUpdateDto.UserId
        };
    }
}
