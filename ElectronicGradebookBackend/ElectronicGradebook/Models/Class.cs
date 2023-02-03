namespace ElectronicGradebook.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassesSubjectsTeachers = new HashSet<ClassesSubjectsTeacher>();
            Lessons = new HashSet<Lesson>();
            Pupils = new HashSet<Pupil>();
        }

        public int ClassId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ClassesSubjectsTeacher> ClassesSubjectsTeachers { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Pupil> Pupils { get; set; }
    }
}
