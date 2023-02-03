using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class TeachingHourRepository : ITeachingHourRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public TeachingHourRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<List<TeachingHour>> SelectTeachingHoursAsync()
        {
            return await _dbContext.TeachingHours
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<byte>> SelectTeachingHoursIdsAsync()
        {
            return await _dbContext.TeachingHours
                .Select(th => th.TeachingHourId)
                .ToListAsync();
        }
    }
}