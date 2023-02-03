using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class WorkdayDetailsToSelectDTO
    {
        public ELessonWorkday Workday { get; set; }
        public DateOnly Date { get; set; }
    }
}
