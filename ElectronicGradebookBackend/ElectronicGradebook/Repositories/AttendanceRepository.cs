using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class AttendanceRepository: IAttendanceRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public AttendanceRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<Class> SelectPupilsAttendancesAsync(DateOnly startDate, DateOnly endDate, int classId)
        {
            DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));

            return await _dbContext.Classes
                .Where(@class => @class.ClassId == classId)
                .Include(@class => @class.Pupils)
                    .ThenInclude(pupil => pupil.User)
                .Include(@class => @class.Pupils)
                    .ThenInclude(pupil => pupil.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                        .ThenInclude(attendance => attendance.User)
                .Include(@class => @class.Pupils)
                    .ThenInclude(pupil => pupil.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                        .ThenInclude(attendance => attendance.Subject)
                .AsNoTracking()
                .FirstAsync();
        }

        public async Task<User> SelectChildrenAttendancesAsync(DateOnly startDate, DateOnly endDate, int parentId)
        {
            DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));

            return await _dbContext.Users
                .Where(user => user.UserId == parentId)
                .Include(user => user.Pupils)
                    .ThenInclude(child => child.User)
                .Include(parent => parent.Pupils)
                    .ThenInclude(child => child.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                        .ThenInclude(attendance => attendance.User)
                .Include(parent => parent.Pupils)
                    .ThenInclude(child => child.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                        .ThenInclude(attendance => attendance.Subject)
                .AsNoTracking()
                .FirstAsync();
        }

        public async Task<Pupil> SelectPupilAttendancesAsync(DateOnly startDate, DateOnly endDate, int pupilId)
        {
            DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));

            return await _dbContext.Pupils
                .Where(pupil => pupil.UserId == pupilId)
                .Include(pupil => pupil.User)
                .Include(pupil => pupil.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                    .ThenInclude(attendance => attendance.User)
                .Include(pupil => pupil.Attendances.Where(attendance => attendance.Date >= startDateTime && attendance.Date <= endDateTime))
                    .ThenInclude(attendance => attendance.Subject)
                .AsNoTracking()
                .FirstAsync();
        }

        public async Task<List<Attendance>> SelectPupilAttendancesAsync(int pupilId)
        {
            return await _dbContext.Attendances
                .Where(attendance => attendance.PupilId == pupilId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> InsertAttendanceAsync(AttendanceDetailsToInsertDTO attendanceDetailsToInsertDTO)
        {
            Attendance attendance = new Attendance()
            {
                Date = attendanceDetailsToInsertDTO.Date.ToDateTime(new TimeOnly(0, 0, 0)),
                IssueDate = DateTime.UtcNow,
                TeachingHourId = attendanceDetailsToInsertDTO.TeachingHourId,
                SubjectId = attendanceDetailsToInsertDTO.SubjectId,
                PupilId = attendanceDetailsToInsertDTO.PupilId,
                UserId = attendanceDetailsToInsertDTO.IssuerId,
                Type = attendanceDetailsToInsertDTO.Type,
            };

            await _dbContext.Attendances.AddAsync(attendance);
            await _dbContext.SaveChangesAsync();
            return attendance.AttendanceId;
        }

        public async Task UpdateAttendanceAsync(AttendanceDetailsToUpdateDTO attendanceDetailsToUpdateDTO)
        {
            var attendance = await _dbContext.Attendances.SingleOrDefaultAsync(a => a.AttendanceId == attendanceDetailsToUpdateDTO.Id);

            if (attendance == null) return;

            attendance.SubjectId = attendanceDetailsToUpdateDTO.SubjectId;
            attendance.Type = attendanceDetailsToUpdateDTO.Type;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAttendanceAsync(int attendanceId)
        {
            _dbContext.Attendances.Remove(new Attendance() { AttendanceId = attendanceId });
            await _dbContext.SaveChangesAsync();
        }
    }
}
