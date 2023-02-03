namespace ElectronicGradebook.DTOs
{
    public class PupilWeeklyAttendanceDetailsToSelectDTO
    {
        public UserShrinkedDetailsToSelectDTO Pupil { get; set; } = null!;
        public Dictionary<DayOfWeek, Dictionary<int, AttendanceDetailsToSelectDTO>> DailyAttendances { get; set; } = null!;
    }
}
