using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AttendanceDetailsToSelectDTO
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public SubjectDetailsToSelectDTO Subject { get; set; } = null!;
        public UserShrinkedDetailsToSelectDTO Issuer { get; set; } = null!;
        public EAttendanceType Type { get; set; }
    }
}
