using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> SelectLessonsAsync(int classId);
        Task InsertLessonAsync(int classId, int teacherId, int subjectId, byte teachingHourId, short classroomId, ELessonWorkday workday);
        Task<List<LessonsException>> SelectLessonExceptionsAsync(DateOnly startDate, DateOnly endDate, IEnumerable<int> lessonIds);
        Task UpdateLessonAsync(int lessonId, int teacherId, int subjectId, short classroomId);
        Task DeleteLessonByIdAsync(int id);
        Task InsertLessonExceptionAsync(DateOnly date, int lessonId, int? teacherId, ELessonsExceptionStatus status);
        Task UpdateLessonExceptionAsync(DateOnly date, int lessonId, int? teacherId, ELessonsExceptionStatus status);
        Task DeleteLessonExceptionAsync(DateOnly date, int lessonId);
    }
}
