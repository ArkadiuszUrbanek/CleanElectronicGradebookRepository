using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AttendanceDetailsToInsertDTO
    {
        public DateOnly Date { get; set; }
        public byte TeachingHourId { get; set; }
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public int IssuerId { get; set; }
        public EAttendanceType Type { get; set; }
    }
}
