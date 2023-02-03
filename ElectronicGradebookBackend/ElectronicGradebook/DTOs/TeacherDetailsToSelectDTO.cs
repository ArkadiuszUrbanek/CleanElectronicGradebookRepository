using System.Text.Json.Serialization;

namespace ElectronicGradebook.DTOs
{
    public class TeacherDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ContactEmail { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ContactNumber { get; set; } = null;
        public IEnumerable<SubjectDetailsToSelectDTO> SubjectsTaught { get; set; } = Enumerable.Empty<SubjectDetailsToSelectDTO>();
    }
}
