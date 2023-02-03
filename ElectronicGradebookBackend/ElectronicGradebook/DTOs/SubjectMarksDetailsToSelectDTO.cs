using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class SubjectMarksDetailsToSelectDTO
    {
        public SubjectDetailsToSelectDTO Subject { get; set; } = null!;
        public IEnumerable<MarkDetailsToSelectDTO> Marks { get; set; } = Enumerable.Empty<MarkDetailsToSelectDTO>();
    }
}
