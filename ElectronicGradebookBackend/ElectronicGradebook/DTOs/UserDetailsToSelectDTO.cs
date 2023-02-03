using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class UserDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EUserRole Role { get; set; }
        public EUserGender Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
