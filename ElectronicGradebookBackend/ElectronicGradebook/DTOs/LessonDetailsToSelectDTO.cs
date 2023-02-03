using ElectronicGradebook.Models.Enums;
using System.Text.Json.Serialization;

namespace ElectronicGradebook.DTOs
{
    public class LessonDetailsToSelectDTO
    {
        public int Id { get; set; }
        public UserDetailsToSelectDTO Teacher { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserDetailsToSelectDTO? SubstituteTeacher { get; set; } = null;
        public SubjectDetailsToSelectDTO Subject { get; set; } = null!;
        public int TeachingHourId { get; set; }
        public ClassroomDetailsToSelectDTO Classroom { get; set; } = null!;
        public ELessonWorkday Workday { get; set; }
        public string Status { get; set; } = null!;
    }
}
