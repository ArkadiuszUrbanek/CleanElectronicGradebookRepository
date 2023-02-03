using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<WeeklyAttendanceDetailsToSelectDTO> SelectWeeklyAttendacesAsync(EUserRole userRole, int userId, DateOnly clientDate, int? classId);
        Task<int> InsertAttendaceAsync(AttendanceDetailsToInsertDTO attendanceDetailsToInsertDTO);
        Task UpdateAttendaceAsync(AttendanceDetailsToUpdateDTO attendanceDetailsToUpdateDTO);
        Task DeleteAttendaceAsync(int attendanceId);
        Task<ICollection<AttendanceStatisticalDataToSelectDTO>> SelectPupilAttendanceMonthlyStatisticalDataAsync(int pupilId);
    }
}
