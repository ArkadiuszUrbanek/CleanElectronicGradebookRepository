namespace ElectronicGradebook.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Attendances = new HashSet<Attendance>();
            ClassesSubjectsTeachers = new HashSet<ClassesSubjectsTeacher>();
            Lessons = new HashSet<Lesson>();
            Marks = new HashSet<Mark>();
        }

        public int SubjectId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<ClassesSubjectsTeacher> ClassesSubjectsTeachers { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
