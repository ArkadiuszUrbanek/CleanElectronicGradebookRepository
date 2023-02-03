using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface ILessonService
    {
        Task<WeeklyTimetableDetailsToSelectDTO> SelectLessonsAsync(DateOnly clientDate, int classId);
        Task InsertLessonAsync(LessonDetailsToInsertDTO lessonDetailsToInsertDTO);
        Task UpdateLessonAsync(LessonDetailsToUpdateDTO lessonDetailsToUpdateDTO);
        Task DeleteLessonByIdAsync(int id);
        Task InsertLessonExceptionAsync(LessonExceptionDetailsToInsertDTO lessonExceptionDetailsToInsertDTO);
        Task UpdateLessonExceptionAsync(LessonExceptionDetailsToUpdateDTO lessonExceptionDetailsToUpdateDTO);
        Task DeleteLessonExceptionAsync(DateOnly date, int lessonId);
    }
}
