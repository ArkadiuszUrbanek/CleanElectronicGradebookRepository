namespace ElectronicGradebook.DTOs
{
    public class AnswerStatisticalDataToSelectDTO
    {
        public int Number { get; set; }
        public string Contents { get; set; } = null!;
        public short TimesSelected { get; set; }
    }
}
