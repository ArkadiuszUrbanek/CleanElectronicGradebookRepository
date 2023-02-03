namespace ElectronicGradebook.DTOs
{
    public class WeeklyTimetableDetailsToSelectDTO
    {
        public List<WorkdayDetailsToSelectDTO> Workdays { get; set; } = new List<WorkdayDetailsToSelectDTO>();
        public List<TeachingHourDetailsToSelectDTO> TeachingHours { get; set; } = new List<TeachingHourDetailsToSelectDTO>();
        public List<LessonDetailsToSelectDTO> Lessons { get; set; } = new List<LessonDetailsToSelectDTO>();
    }
}
