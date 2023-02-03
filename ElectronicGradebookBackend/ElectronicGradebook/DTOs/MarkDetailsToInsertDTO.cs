using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class MarkDetailsToInsertDTO
    {
        public decimal Value { get; set; }
        public int? Weight { get; set; }
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public int IssuerId { get; set; }
        public EMarkSemester? Semester { get; set; }
        public EMarkType Type { get; set; }
        public EMarkCategory? Category { get; set; }
    }
}
