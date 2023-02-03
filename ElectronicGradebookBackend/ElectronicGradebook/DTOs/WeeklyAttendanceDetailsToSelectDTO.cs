namespace ElectronicGradebook.DTOs
{
    public class WeeklyAttendanceDetailsToSelectDTO
    {
        public List<WorkdayDetailsToSelectDTO> Workdays { get; set; } = new List<WorkdayDetailsToSelectDTO>();
        public List<byte> TeachingHoursIds { get; set; } = new List<byte>();
        public IEnumerable<PupilWeeklyAttendanceDetailsToSelectDTO> PupilsWeeklyAttendances { get; set; } = Enumerable.Empty<PupilWeeklyAttendanceDetailsToSelectDTO>();
    }
}
