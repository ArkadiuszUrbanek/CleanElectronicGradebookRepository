namespace ElectronicGradebook.Models
{
    public partial class TeachingHour
    {
        public TeachingHour()
        {
            Attendances = new HashSet<Attendance>();
            Lessons = new HashSet<Lesson>();
        }

        public byte TeachingHourId { get; set; }
        public TimeSpan StartTime { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
