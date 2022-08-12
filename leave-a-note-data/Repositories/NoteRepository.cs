using leave_a_note_data.Entities;
using Microsoft.EntityFrameworkCore;

namespace leave_a_note_data.Repositories
{
    public class NoteRepository : IRepository<Note>
    {
        private readonly LeaveANoteDbContext _context;

        public NoteRepository(LeaveANoteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await _context.Notes.AsNoTracking().ToListAsync();
        }

        public async Task<Note> GetAsync(Guid id)
        {
            return await _context.Notes.AsNoTracking().FirstAsync(note => note.Id == id);
        }

        public async Task<Note> UpdateAsync(Note entity)
        {
            var noteToUpdate = await GetAsync(entity.Id);
            noteToUpdate.NoteText = entity.NoteText;
            noteToUpdate.PublishDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return noteToUpdate;
        }

        public async Task DeleteAsync(Guid id)
        {
            _context.Notes.Remove(await GetAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Note entity)
        {
            await _context.Notes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
