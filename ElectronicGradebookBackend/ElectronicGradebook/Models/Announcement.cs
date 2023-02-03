namespace ElectronicGradebook.Models
{
    public partial class Announcement
    {
        public Announcement()
        {
            AnnouncementRoles = new HashSet<AnnouncementRole>();
        }

        public int AnnouncementId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<AnnouncementRole> AnnouncementRoles { get; set; }
    }
}
