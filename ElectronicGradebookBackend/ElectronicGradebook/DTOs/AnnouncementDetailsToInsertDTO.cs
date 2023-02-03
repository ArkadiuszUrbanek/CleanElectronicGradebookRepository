using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AnnouncementDetailsToInsertDTO
    {
        public string Title { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public int AuthorId { get; set; }
        public HashSet<EUserRole> AuthorizedRoles { get; set; } = new HashSet<EUserRole>();
    }
}
