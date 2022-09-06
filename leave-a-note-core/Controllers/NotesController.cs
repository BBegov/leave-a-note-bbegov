using System.Runtime.CompilerServices;
using leave_a_note_core.Models.DTOs;
using leave_a_note_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace leave_a_note_core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<NoteViewDto>>> GetAllNotes()
    {
        return await _noteService.GetAllNotesAsync();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("{id:int}", Name = "GetNote")]
    public async Task<ActionResult<NoteViewDto>> GetNote(int id)
    {
        try
        {
            return await _noteService.GetNoteByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"No note with ID:{id} found.");
        }
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost]
    public async Task<ActionResult<NoteViewDto>> AddNote(NoteCreateDto newNote)
    {
        try
        {
            var createdNote = await _noteService.AddNoteAsync(newNote);
            return CreatedAtRoute("GetNote", new { id = createdNote.Id }, createdNote);
        }
        catch (RuntimeWrappedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<NoteViewDto>> UpdateNote(NoteUpdateDto updatedNote, int id)
    {
        updatedNote.Id = id;

        try
        {
            return await _noteService.UpdateNoteAsync(updatedNote);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"No note with ID:{id} found.");
        }
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteNote(int id)
    {
        try
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
        catch (RuntimeWrappedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"No note with ID:{id} found.");
        }
    }
}
