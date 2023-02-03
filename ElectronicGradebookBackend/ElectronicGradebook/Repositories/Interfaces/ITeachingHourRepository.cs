using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface ITeachingHourRepository
    {
        Task<List<TeachingHour>> SelectTeachingHoursAsync();
        Task<List<byte>> SelectTeachingHoursIdsAsync();
    }
}
