namespace ElectronicGradebook.Models
{
    public partial class ClassesSubjectsTeacher
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
    }
}
