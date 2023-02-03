using ElectronicGradebook.DTOs;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<SubjectDetailsToSelectDTO>> SelectSubjectsAsync()
        {
            return (await _subjectRepository.SelectSubjectsAsync())
                .Select(s => new SubjectDetailsToSelectDTO()
                {
                    Id = s.SubjectId,
                    Name = s.Name
                }
            );
        }

        public async Task<IEnumerable<SubjectDetailsToSelectDTO>> SelectSubjectsTaughtByTeacherAsync(int teacherId, int classId)
        {
            return (await _subjectRepository.SelectSubjectsTaughtByTeacherAsync(teacherId, classId))
                .ClassesSubjectsTeachers
                .Select(classTeacherSubject => new SubjectDetailsToSelectDTO()
                    {
                        Id = classTeacherSubject.SubjectId,
                        Name = classTeacherSubject.Subject.Name
                    }
                );
        }
    }
}
