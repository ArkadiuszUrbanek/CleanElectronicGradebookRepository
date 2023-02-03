using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AnnouncementDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public UserDetailsToSelectDTO Author { get; set; } = null!;
        public HashSet<EUserRole> AuthorizedRoles { get; set; } = new HashSet<EUserRole>();
    }
}
