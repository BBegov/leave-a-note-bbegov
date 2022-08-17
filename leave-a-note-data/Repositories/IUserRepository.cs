using leave_a_note_data.Entities;

namespace leave_a_note_data.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetAsync(int id);
    Task<User> UpdateAsync(User entity);
    Task DeleteAsync(int id);
    Task AddAsync(User entity);
    Task<User> UpdateWithPasswordAsync(User entity);

}