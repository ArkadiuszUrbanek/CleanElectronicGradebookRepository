namespace ElectronicGradebook.Models
{
    public partial class Post
    {
        public Post()
        {
            PostReactions = new HashSet<PostReaction>();
            PostRoles = new HashSet<PostRole>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Contents { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<PostReaction> PostReactions { get; set; }
        public virtual ICollection<PostRole> PostRoles { get; set; }
    }
}
