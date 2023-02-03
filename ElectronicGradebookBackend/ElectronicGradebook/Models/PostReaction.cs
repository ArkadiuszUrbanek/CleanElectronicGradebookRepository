using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class PostReaction
    {
        public int PostReactionId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public EPostReaction Type { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
