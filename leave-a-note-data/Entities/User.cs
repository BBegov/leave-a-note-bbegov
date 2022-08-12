using System.ComponentModel.DataAnnotations;

namespace leave_a_note_data.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string PasswrodHash { get; set; }
        
        [Required]
        public UserRole Role { get; set; }

        public List<Note> Notes { get; set; }
    }
}