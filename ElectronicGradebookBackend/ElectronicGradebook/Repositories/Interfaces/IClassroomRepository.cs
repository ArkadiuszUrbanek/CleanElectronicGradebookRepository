using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IClassroomRepository
    {
        Task<List<Classroom>> SelectClassroomsAsync();
    }
}
