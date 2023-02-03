using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class Mark
    {
        public int MarkId { get; set; }
        public decimal Value { get; set; }
        public int? Weight { get; set; }
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public int UserId { get; set; }
        public EMarkSemester? Semester { get; set; }
        public DateTime IssueDate { get; set; }
        public EMarkType Type { get; set; }
        public EMarkCategory? Category { get; set; }

        public virtual Pupil Pupil { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
