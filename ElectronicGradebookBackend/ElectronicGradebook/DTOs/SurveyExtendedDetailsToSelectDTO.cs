namespace ElectronicGradebook.DTOs
{
    public class SurveyExtendedDetailsToSelectDTO : SurveyDetailsToSelectDTO
    {
        public IEnumerable<QuestionDetailsToSelectDTO> Questions { get; set; } = Enumerable.Empty<QuestionDetailsToSelectDTO>();
    }
}
