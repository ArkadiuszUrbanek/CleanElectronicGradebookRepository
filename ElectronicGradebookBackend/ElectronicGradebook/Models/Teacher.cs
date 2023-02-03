namespace ElectronicGradebook.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            ClassesSubjectsTeachers = new HashSet<ClassesSubjectsTeacher>();
            Lessons = new HashSet<Lesson>();
            LessonsExceptions = new HashSet<LessonsException>();
        }

        public int UserId { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<ClassesSubjectsTeacher> ClassesSubjectsTeachers { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<LessonsException> LessonsExceptions { get; set; }
    }
}
