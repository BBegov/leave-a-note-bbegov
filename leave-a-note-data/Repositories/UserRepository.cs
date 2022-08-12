using leave_a_note_data.Entities;
using Microsoft.EntityFrameworkCore;

namespace leave_a_note_data.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly LeaveANoteDbContext _context;

    public UserRepository(LeaveANoteDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.Include(user => user.Notes).AsNoTracking().ToListAsync();
    }

    public async Task<User> GetAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstAsync(user => user.Id == id);
    }

    public async Task<User> UpdateAsync(User entity)
    {
        var userToUpdate = await GetAsync(entity.Id);
        userToUpdate.UserName = entity.UserName;
        userToUpdate.FirstName = entity.FirstName;
        userToUpdate.LastName = entity.LastName;
        userToUpdate.PasswordHash = entity.PasswordHash;
        await _context.SaveChangesAsync();
        return userToUpdate;
    }

    public async Task DeleteAsync(int id)
    {
        _context.Users.Remove(await GetAsync(id));
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}