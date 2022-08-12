using leave_a_note_data.Entities;

namespace leave_a_note_data
{
    public class DataSeeder
    {
        private readonly LeaveANoteDbContext _context;

        public DataSeeder(LeaveANoteDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (_context.Users.Any()) return;

            var users = new User[]
            {
                new()
                {
                    UserName = "MainAdmin",
                    FirstName = "Main",
                    LastName = "Admin",
                    PasswrodHash = BCrypt.Net.BCrypt.HashPassword("asdf1234"),
                    Role = UserRole.Admin
                }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();

            var notes = new Note[]
            {
                new()
                {
                    NoteText = "Hello, this is the first note!",
                    PublishDate = DateTime.Now,
                    User = users[0]
                }
            };

            _context.Notes.AddRange(notes);
            _context.SaveChanges();
        }
    }
}
