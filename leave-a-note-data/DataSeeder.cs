﻿using leave_a_note_data.Entities;

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
                    FirstName = "John",
                    LastName = "Doe",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("asdf1234"),
                    Role = UserRole.Admin
                },
                new()
                {
                    UserName = "FirstUser",
                    FirstName = "Simon",
                    LastName = "Mathias",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("fdsa1234"),
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
                    NoteText = "Second message from Simon",
                    PublishDate = DateTime.Now,
                    UserId = 2
                },
                new()
                {
                    NoteText = "Test message from John",
                    PublishDate = DateTime.Now,
                    UserId = 1
                }
        };

        _context.Notes.AddRange(notes);
        _context.SaveChanges();
    }
}
