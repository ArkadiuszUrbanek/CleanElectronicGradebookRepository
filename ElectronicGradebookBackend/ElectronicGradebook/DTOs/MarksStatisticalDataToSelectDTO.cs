namespace ElectronicGradebook.DTOs
{
    public class MarksStatisticalDataToSelectDTO
    {
        public string Label { get; set; } = null!;
        public decimal PupilWeightedAverage { get; set; }
        public decimal ClassWeightedAverage { get; set; }
    }
}