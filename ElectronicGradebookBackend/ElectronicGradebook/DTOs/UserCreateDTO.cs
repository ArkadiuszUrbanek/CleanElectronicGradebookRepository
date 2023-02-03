using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class UserCreateDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EUserRole Role { get; set; }
        public EUserGender Gender { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? SecondName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
    }
}
