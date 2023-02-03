using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class LessonDetailsToInsertDTO
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public byte TeachingHourId { get; set; }
        public short ClassroomId { get; set; }
        public ELessonWorkday Workday { get; set; }
    }
}
