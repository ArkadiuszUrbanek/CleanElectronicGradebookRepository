using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class AttendanceStatisticalDataToSelectDTO
    {
        public string Label { get; set; } = null!;
        public Dictionary<EAttendanceType, int> Data { get; set; } = new Dictionary<EAttendanceType, int>();
    }
}