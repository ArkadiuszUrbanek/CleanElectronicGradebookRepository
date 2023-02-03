using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class QuestionDetailsToSelectDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Contents { get; set; } = null!;
	    public EQuestionType Type { get; set; }
        public IEnumerable<AnswerDetailsToSelectDTO> Answers { get; set; } = Enumerable.Empty<AnswerDetailsToSelectDTO>();
    }
}
