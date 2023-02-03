using ElectronicGradebook.Models.Enums;
using System.Text.Json.Serialization;

namespace ElectronicGradebook.DTOs
{
    public class PostDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string Contents { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public UserDetailsToSelectDTO Author { get; set; } = null!;
        public HashSet<EUserRole> AuthorizedRoles { get; set; } = new HashSet<EUserRole>();
        public Dictionary<EPostReaction, int> UsersReactions { get; set; } = new Dictionary<EPostReaction, int>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EPostReaction? CurrentUserReaction { get; set; } = null;
    }
}
