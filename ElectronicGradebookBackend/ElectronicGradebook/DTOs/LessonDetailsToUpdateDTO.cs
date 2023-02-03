using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class LessonDetailsToUpdateDTO
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public short ClassroomId { get; set; }
    }
}
