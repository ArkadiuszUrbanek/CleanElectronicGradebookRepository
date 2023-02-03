using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public ClassroomRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Classroom>> SelectClassroomsAsync()
        { 
            return await _dbContext.Classrooms
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
