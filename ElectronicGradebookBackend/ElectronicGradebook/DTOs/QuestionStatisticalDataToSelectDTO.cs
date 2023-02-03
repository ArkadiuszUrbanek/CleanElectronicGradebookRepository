using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class QuestionStatisticalDataToSelectDTO
    {
        public int Number { get; set; }
        public string Contents { get; set; } = null!;
        public EQuestionType Type { get; set; }
        public IEnumerable<AnswerStatisticalDataToSelectDTO> Answers { get; set; } = Enumerable.Empty<AnswerStatisticalDataToSelectDTO>();
    }
}
