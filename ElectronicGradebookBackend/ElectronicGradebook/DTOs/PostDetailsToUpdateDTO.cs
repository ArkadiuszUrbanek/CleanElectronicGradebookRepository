using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class PostDetailsToUpdateDTO
    {
        public int Id { get; set; }
        public string? Contents { get; set; }
        public HashSet<EUserRole>? AuthorizedRoles { get; set; }
    }
}
