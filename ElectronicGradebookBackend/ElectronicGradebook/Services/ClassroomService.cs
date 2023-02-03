using ElectronicGradebook.DTOs;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task<IEnumerable<ClassroomDetailsToSelectDTO>> SelectClassroomsAsync()
        {
            return (await _classroomRepository.SelectClassroomsAsync())
                .Select(c => new ClassroomDetailsToSelectDTO()
                {
                    Id = c.ClassroomId,
                    FloorNumber = c.FloorNumber
                }
            );
        }
    }
}
