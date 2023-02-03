namespace ElectronicGradebook.Models
{
    public partial class Survey
    {
        public Survey()
        {
            Questions = new HashSet<Question>();
            UsersSurveys = new HashSet<UsersSurvey>();
        }

        public int SurveyId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UsersSurvey> UsersSurveys { get; set; }
    }
}
