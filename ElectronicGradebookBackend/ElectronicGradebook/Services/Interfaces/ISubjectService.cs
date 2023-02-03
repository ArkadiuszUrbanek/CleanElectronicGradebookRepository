using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDetailsToSelectDTO>> SelectSubjectsAsync();
        Task<IEnumerable<SubjectDetailsToSelectDTO>> SelectSubjectsTaughtByTeacherAsync(int teacherId, int classId);
    }
}
