using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AttendanceDetailsToUpdateDTO
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public EAttendanceType Type { get; set; }
    }
}