using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDetailsToSelectDTO>> SelectClassesAsync();
        Task<IEnumerable<ClassDetailsToSelectDTO>> SelectClassesTaughtByTeacherAsync(int teacherId);
    }
}
