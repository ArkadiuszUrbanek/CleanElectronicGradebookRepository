using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            LessonsExceptions = new HashSet<LessonsException>();
        }

        public int LessonId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public byte TeachingHourId { get; set; }
        public short ClassroomId { get; set; }
        public ELessonWorkday Workday { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Classroom Classroom { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual TeachingHour TeachingHour { get; set; } = null!;
        public virtual ICollection<LessonsException> LessonsExceptions { get; set; }
    }
}
