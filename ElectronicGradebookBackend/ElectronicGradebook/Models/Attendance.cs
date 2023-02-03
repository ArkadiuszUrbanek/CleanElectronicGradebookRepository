using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime IssueDate { get; set; }
        public byte TeachingHourId { get; set; }
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public int UserId { get; set; }
        public EAttendanceType Type { get; set; }

        public virtual Pupil Pupil { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual TeachingHour TeachingHour { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}