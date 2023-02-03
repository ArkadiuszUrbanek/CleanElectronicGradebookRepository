namespace ElectronicGradebook.DTOs
{
    public class SurveyStatisticalDataToSelectDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int TimesFinished { get; set; }
        public int TimesUnfinished { get; set; }
        public IEnumerable<QuestionStatisticalDataToSelectDTO> Questions { get; set; } = Enumerable.Empty<QuestionStatisticalDataToSelectDTO>();
    }
}
