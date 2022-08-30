using leave_a_note_data.Entities;

namespace leave_a_note_data;

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
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("asdf1234"),
                    Role = UserRole.Admin
                },
                new()
                {
                    UserName = "SimpleUser",
                    FirstName = "Bob",
                    LastName = "Black",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("asdf1234"),
                    Role = UserRole.User
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
                    UserId = 1
                },
                new()
                {
                    NoteText = "Second note for testing",
                    PublishDate = DateTime.Now,
                    UserId = 2
                },
                new()
                {
                    NoteText = "Third note for testing",
                    PublishDate = DateTime.Now,
                    UserId = 1
                }
        };

        _context.Notes.AddRange(notes);
        _context.SaveChanges();
    }
}
