using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public byte Number { get; set; }
        public string Contents { get; set; } = null!;
        public EQuestionType Type { get; set; }
        public int SurveyId { get; set; }

        public virtual Survey Survey { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
