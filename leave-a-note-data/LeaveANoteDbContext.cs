using leave_a_note_data.Entities;
using Microsoft.EntityFrameworkCore;

namespace leave_a_note_data
{
    public class LeaveANoteDbContext : DbContext
    {
        public LeaveANoteDbContext(DbContextOptions<LeaveANoteDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>().ToTable("Note");
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}