using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IClassroomService
    {
        Task<IEnumerable<ClassroomDetailsToSelectDTO>> SelectClassroomsAsync();
    }
}
