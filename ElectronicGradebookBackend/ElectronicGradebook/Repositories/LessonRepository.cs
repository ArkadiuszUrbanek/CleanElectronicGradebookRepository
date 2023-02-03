using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class LessonRepository: ILessonRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public LessonRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Lesson>> SelectLessonsAsync(int classId)
        {
            return await _dbContext.Lessons
                .Where(l => l.ClassId == classId)
                .Include(l => l.Subject)
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .ThenInclude(t => t.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task InsertLessonAsync(int classId, int teacherId, int subjectId, byte teachingHourId, short classroomId, ELessonWorkday workday)
        {
            await _dbContext.Lessons.AddAsync(new Lesson() 
                {
                    ClassId = classId,
                    TeacherId = teacherId,
                    SubjectId = subjectId,
                    TeachingHourId = teachingHourId,
                    ClassroomId = classroomId,
                    Workday = workday
                }
            );
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<LessonsException>> SelectLessonExceptionsAsync(DateOnly startDate, DateOnly endDate, IEnumerable<int> lessonIds)
        {
            DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));

            return await _dbContext.LessonsExceptions
                .Where(le => le.Date >= startDateTime &&
                             le.Date <= endDateTime &&
                             lessonIds.Contains(le.LessonId))
                .Include(le => le.Teacher)
                .ThenInclude(t => t.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateLessonAsync(int lessonId, int teacherId, int subjectId, short classroomId)
        {
            var foundLesson = await _dbContext.Lessons.SingleOrDefaultAsync(l => l.LessonId == lessonId);
            if (foundLesson != null)
            {
                foundLesson.TeacherId = teacherId;
                foundLesson.SubjectId = subjectId;
                foundLesson.ClassroomId = classroomId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteLessonByIdAsync(int id)
        {
            _dbContext.Lessons.Remove(new Lesson() { LessonId = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertLessonExceptionAsync(DateOnly date, int lessonId, int? teacherId, ELessonsExceptionStatus status)
        {
            await _dbContext.LessonsExceptions.AddAsync(new LessonsException()
            {
                LessonId = lessonId,
                TeacherId = teacherId,
                Date = date.ToDateTime(new TimeOnly(0, 0, 0)),
                Status = status
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateLessonExceptionAsync(DateOnly date, int lessonId, int? teacherId, ELessonsExceptionStatus status)
        {
            DateTime dateTime = date.ToDateTime(new TimeOnly(0, 0, 0));
            var foundLessonexception = await _dbContext.LessonsExceptions.SingleOrDefaultAsync(le => le.Date == dateTime && le.LessonId == lessonId);
            if (foundLessonexception != null)
            {
                foundLessonexception.TeacherId = teacherId ?? foundLessonexception.TeacherId;
                foundLessonexception.Status = status;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteLessonExceptionAsync(DateOnly date, int lessonId)
        {
            DateTime dateTime = date.ToDateTime(new TimeOnly(0, 0, 0));
            LessonsException? lessonException = await _dbContext.LessonsExceptions.SingleOrDefaultAsync(le => le.Date == dateTime && le.LessonId == lessonId);

            if (lessonException != null)
            {
                _dbContext.LessonsExceptions.Remove(lessonException);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
