using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class PostReactionDetailsToInsertDTO
    {
        public int Id { get; set; }
        public EPostReaction Type { get; set; }
    }
}
