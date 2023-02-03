namespace ElectronicGradebook.Models
{
    public partial class UsersSurvey
    {
        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? CompletionDate { get; set; }

        public virtual Survey Survey { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
