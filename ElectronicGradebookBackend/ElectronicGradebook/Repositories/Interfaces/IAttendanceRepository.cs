using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Class> SelectPupilsAttendancesAsync(DateOnly startDate, DateOnly endDate, int classId);
        Task<User> SelectChildrenAttendancesAsync(DateOnly startDate, DateOnly endDate, int parentId);
        Task<Pupil> SelectPupilAttendancesAsync(DateOnly startDate, DateOnly endDate, int pupilId);
        Task<List<Attendance>> SelectPupilAttendancesAsync(int pupilId);
        Task<int> InsertAttendanceAsync(AttendanceDetailsToInsertDTO attendanceDetailsToInsertDTO);
        Task UpdateAttendanceAsync(AttendanceDetailsToUpdateDTO attendanceDetailsToUpdateDTO);
        Task DeleteAttendanceAsync(int pupilId);
    }
}
