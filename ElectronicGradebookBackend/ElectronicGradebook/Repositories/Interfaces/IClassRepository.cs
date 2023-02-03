using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<List<Class>> SelectClassesAsync();
        Task<List<Class>> SelectClassesTaughtByTeacherAsync(int teacherId);
    }
}
