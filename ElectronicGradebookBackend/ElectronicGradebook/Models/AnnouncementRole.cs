using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class AnnouncementRole
    {
        public int AnnouncementId { get; set; }
        public EUserRole Role { get; set; }

        public virtual Announcement Announcement { get; set; } = null!;
    }
}
