using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> SelectSubjectsAsync();
        Task<Teacher> SelectSubjectsTaughtByTeacherAsync(int teacherId, int classId);
    }
}
