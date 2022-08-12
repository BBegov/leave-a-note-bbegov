namespace leave_a_note_data.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task AddAsync(T entity);
    }
}
