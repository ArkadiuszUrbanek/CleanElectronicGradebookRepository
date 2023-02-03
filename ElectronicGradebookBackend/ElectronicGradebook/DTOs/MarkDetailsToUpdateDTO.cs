using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class MarkDetailsToUpdateDTO
    {
        public int Id { get; set; }
        public decimal? Value { get; set; }
        public int? Weight { get; set; }
        public EMarkCategory? Category { get; set; }
    }
}
