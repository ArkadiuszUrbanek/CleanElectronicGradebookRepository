using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class PostRole
    {
        public int PostId { get; set; }
        public EUserRole Role { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
