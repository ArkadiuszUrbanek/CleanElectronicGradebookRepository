using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class LessonExceptionDetailsToInsertDTO
    {
        public DateOnly Date { get; set; }
        public int LessonId { get; set; }
        public int? TeacherId { get; set; }
        public ELessonsExceptionStatus Status { get; set; }
    }
}
