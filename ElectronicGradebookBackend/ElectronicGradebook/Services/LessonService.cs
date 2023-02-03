using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ITeachingHourRepository _teachinghourRepository;
        public LessonService(ILessonRepository lessonRepository, ITeachingHourRepository teachinghourRepository)
        {
            _lessonRepository = lessonRepository;
            _teachinghourRepository = teachinghourRepository;
        }

        public async Task<WeeklyTimetableDetailsToSelectDTO> SelectLessonsAsync(DateOnly clientDate, int classId)
        {
            int diff = (7 + (clientDate.DayOfWeek - DayOfWeek.Monday)) % 7;

            List<WorkdayDetailsToSelectDTO> workdaysDetails = new List<WorkdayDetailsToSelectDTO>();

            ELessonWorkday[] workdaysEnums = Enum.GetValues<ELessonWorkday>();
            for (int i = 0; i < workdaysEnums.Length; i++)
            {
                workdaysDetails.Add(new WorkdayDetailsToSelectDTO()
                {
                    Workday = workdaysEnums[i],
                    Date = clientDate.AddDays(-1 * diff + i)
                });
            }

            var teachingHours = await _teachinghourRepository.SelectTeachingHoursAsync();
            List<TeachingHourDetailsToSelectDTO> teachingHoursDetails = new List<TeachingHourDetailsToSelectDTO>();
            teachingHoursDetails.AddRange(teachingHours.Select(th => new TeachingHourDetailsToSelectDTO()
            {
                Id = th.TeachingHourId,
                StartTime = TimeOnly.FromTimeSpan(th.StartTime)
            })
            );

            var lessons = await _lessonRepository.SelectLessonsAsync(classId);

            var lessonsExceptions = await _lessonRepository.SelectLessonExceptionsAsync(clientDate.AddDays(-1 * diff), clientDate.AddDays(-1 * diff + 4), lessons.Select(l => l.LessonId));

            List<LessonDetailsToSelectDTO> lessonsDetails = new List<LessonDetailsToSelectDTO>();
            lessonsDetails.AddRange(
                lessons.Select(l =>
                {
                    LessonsException? lessonException = lessonsExceptions.FirstOrDefault(le => le.LessonId == l.LessonId);

                    return new LessonDetailsToSelectDTO()
                    {
                        Id = l.LessonId,
                        Teacher = new UserDetailsToSelectDTO()
                        {
                            Id = l.TeacherId,
                            FirstName = l.Teacher.User.FirstName,
                            LastName = l.Teacher.User.LastName,
                            Role = l.Teacher.User.Role,
                            Gender = l.Teacher.User.Gender,
                            IsActive = l.Teacher.User.IsActive
                        },
                        SubstituteTeacher = lessonException == null || lessonException.Status == ELessonsExceptionStatus.Cancelled ? null : new UserDetailsToSelectDTO()
                        {
                            Id = (int)lessonException.TeacherId,
                            FirstName = lessonException.Teacher.User.FirstName,
                            LastName = lessonException.Teacher.User.LastName,
                            Role = lessonException.Teacher.User.Role,
                            Gender = lessonException.Teacher.User.Gender,
                            IsActive = lessonException.Teacher.User.IsActive
                        },
                        Subject = new SubjectDetailsToSelectDTO()
                        {
                            Id = l.Subject.SubjectId,
                            Name = l.Subject.Name,
                        },
                        TeachingHourId = l.TeachingHourId,
                        Classroom = new ClassroomDetailsToSelectDTO()
                        {
                            Id = l.Classroom.ClassroomId,
                            FloorNumber = l.Classroom.FloorNumber
                        },
                        Workday = l.Workday,
                        Status = lessonException == null ? "AsPlanned" : lessonException.Status.ToString()
                    };
                }
                )
            );

            return new WeeklyTimetableDetailsToSelectDTO()
            {
                Workdays = workdaysDetails,
                TeachingHours = teachingHoursDetails,
                Lessons = lessonsDetails
            };
        }

        public async Task InsertLessonAsync(LessonDetailsToInsertDTO lessonDetailsToInsertDTO)
        {
            await _lessonRepository.InsertLessonAsync(lessonDetailsToInsertDTO.ClassId,
                                                      lessonDetailsToInsertDTO.TeacherId,
                                                      lessonDetailsToInsertDTO.SubjectId,
                                                      lessonDetailsToInsertDTO.TeachingHourId,
                                                      lessonDetailsToInsertDTO.ClassroomId,
                                                      lessonDetailsToInsertDTO.Workday);
        }

        public async Task UpdateLessonAsync(LessonDetailsToUpdateDTO lessonDetailsToUpdateDTO)
        {
            await _lessonRepository.UpdateLessonAsync(lessonDetailsToUpdateDTO.Id,
                                                      lessonDetailsToUpdateDTO.TeacherId,
                                                      lessonDetailsToUpdateDTO.SubjectId,
                                                      lessonDetailsToUpdateDTO.ClassroomId);
        }

        public async Task DeleteLessonByIdAsync(int id)
        {
            await _lessonRepository.DeleteLessonByIdAsync(id);
        }

        public async Task InsertLessonExceptionAsync(LessonExceptionDetailsToInsertDTO lessonExceptionDetailsToInsertDTO)
        {
            await _lessonRepository.InsertLessonExceptionAsync(lessonExceptionDetailsToInsertDTO.Date,
                                                               lessonExceptionDetailsToInsertDTO.LessonId,
                                                               lessonExceptionDetailsToInsertDTO.TeacherId,
                                                               lessonExceptionDetailsToInsertDTO.Status);
        }

        public async Task UpdateLessonExceptionAsync(LessonExceptionDetailsToUpdateDTO lessonExceptionDetailsToUpdateDTO)
        {
            await _lessonRepository.UpdateLessonExceptionAsync(lessonExceptionDetailsToUpdateDTO.Date,
                                                               lessonExceptionDetailsToUpdateDTO.LessonId,
                                                               lessonExceptionDetailsToUpdateDTO.TeacherId,
                                                               lessonExceptionDetailsToUpdateDTO.Status);
        }

        public async Task DeleteLessonExceptionAsync(DateOnly date, int lessonId)
        {
            await _lessonRepository.DeleteLessonExceptionAsync(date, lessonId);
        }
    }
}
