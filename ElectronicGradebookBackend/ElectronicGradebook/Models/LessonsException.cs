using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class LessonsException
    {
        public int LessonExceptionId { get; set; }
        public int LessonId { get; set; }
        public int? TeacherId { get; set; }
        public DateTime Date { get; set; }
        public ELessonsExceptionStatus Status { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Teacher? Teacher { get; set; }
    }
}
