using ElectronicGradebook.Models.Enums;
using System.Text.Json.Serialization;

namespace ElectronicGradebook.DTOs
{
    public class MarkDetailsToSelectDTO
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Weight { get; set; } = null;
        public UserDetailsToSelectDTO Issuer { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EMarkSemester? Semester { get; set; } = null;
        public DateTime IssueDate { get; set; }
        public EMarkType Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EMarkCategory? Category { get; set; } = null;
    }
}