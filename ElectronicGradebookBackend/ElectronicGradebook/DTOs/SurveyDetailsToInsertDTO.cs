using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class SurveyDetailsToInsertDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int AuthorId { get; set; }
        public EUserRole[] TargetedRoles { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }

        public class QuestionDetailsToInsertDTO
        {
            public string Contents { get; set; } = null!;
	        public EQuestionType Type { get; set; }
            public string[] AnswersContents { get; set; } = null!;
        }

        public QuestionDetailsToInsertDTO[] Questions { get; set; } = null!;
    }
}
