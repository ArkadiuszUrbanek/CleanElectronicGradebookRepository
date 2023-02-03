using ElectronicGradebook.DTOs;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<ClassDetailsToSelectDTO>> SelectClassesAsync()
        {
            return (await _classRepository.SelectClassesAsync())
                .Select(c => new ClassDetailsToSelectDTO()
                {
                    Id = c.ClassId,
                    Name = c.Name
                });
        }

        public async Task<IEnumerable<ClassDetailsToSelectDTO>> SelectClassesTaughtByTeacherAsync(int teacherId)
        {
            return (await _classRepository.SelectClassesTaughtByTeacherAsync(teacherId))
                .Select(c => new ClassDetailsToSelectDTO()
                {
                    Id = c.ClassId,
                    Name = c.Name
                }); ;
        }
    }
}
