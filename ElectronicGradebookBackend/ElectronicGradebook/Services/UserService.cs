using ElectronicGradebook.DTOs;
using ElectronicGradebook.Services.Interfaces;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Models;
using System.Security.Cryptography;
using System.Text;
using ElectronicGradebook.Repositories.Interfaces;

namespace ElectronicGradebook.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEMailService _eMailService;

        public UserService(IUserRepository userRepository, IEMailService eMailService)
        {
            _userRepository = userRepository;
            _eMailService = eMailService;
        }

        public async Task<int> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            int recordId;
            switch (userCreateDTO.Role)
            {
                case EUserRole.Pupil:
                {
                    var hmac = new HMACSHA512();

                    recordId = await _userRepository.InsertPupilAsync(
                        new User()
                        {
                            FirstName = userCreateDTO.FirstName,
                            LastName = userCreateDTO.LastName,
                            Role = userCreateDTO.Role,
                            Gender = userCreateDTO.Gender,
                            Email = userCreateDTO.Email,
                            IsActive = true,
                            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDTO.Password)),
                            PasswordSalt = hmac.Key
                        },
                        new Pupil()
                        {
                            SecondName = userCreateDTO.SecondName,
                            BirthDate = userCreateDTO.BirthDate?.ToDateTime(new TimeOnly(0, 0, 0))
                        }
                    );
                    break;
                }

                case EUserRole.Parent:
                {
                    var hmac = new HMACSHA512();

                    recordId = await _userRepository.InsertUserAsync(
                        new User()
                        {
                            FirstName = userCreateDTO.FirstName,
                            LastName = userCreateDTO.LastName,
                            Role = userCreateDTO.Role,
                            Gender = userCreateDTO.Gender,
                            Email = userCreateDTO.Email,
                            IsActive = true,
                            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDTO.Password)),
                            PasswordSalt = hmac.Key
                        }
                    );
                    break;
                }

                case EUserRole.Teacher:
                {
                    var hmac = new HMACSHA512();

                    recordId = await _userRepository.InsertTeacherAsync(
                        new User()
                        {
                            FirstName = userCreateDTO.FirstName,
                            LastName = userCreateDTO.LastName,
                            Role = userCreateDTO.Role,
                            Gender = userCreateDTO.Gender,
                            Email = userCreateDTO.Email,
                            IsActive = true,
                            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDTO.Password)),
                            PasswordSalt = hmac.Key
                        },
                        new Teacher()
                        {
                            ContactEmail = userCreateDTO.ContactEmail,
                            ContactNumber = userCreateDTO.ContactNumber
                        }
                    );
                    break;
                }

                case EUserRole.Admin:
                {
                    var hmac = new HMACSHA512();

                    recordId = await _userRepository.InsertUserAsync(
                        new User()
                        {
                            FirstName = userCreateDTO.FirstName,
                            LastName = userCreateDTO.LastName,
                            Role = userCreateDTO.Role,
                            Gender = userCreateDTO.Gender,
                            Email = userCreateDTO.Email,
                            IsActive = true,
                            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDTO.Password)),
                            PasswordSalt = hmac.Key
                        }
                    );
                    break;
                }

                default: throw new ArgumentOutOfRangeException($"Invalid user role value: {userCreateDTO.Role}");
            }

            StringBuilder emailBodyBuilder = new StringBuilder();
            emailBodyBuilder.Append($"Witaj {userCreateDTO.FirstName} {userCreateDTO.LastName},\n\n");
            emailBodyBuilder.Append("Administrator dziennika elektronicznego właśnie założył konto na twój adres e-mail. Oczekuj przekazania w szkole hasła do twojego konta.");
            await _eMailService.sendEMailAsync("Powiadomienie z aplikacji dziennik elektroniczny", userCreateDTO.Email, emailBodyBuilder.ToString());
            return recordId;
        }

        public UserPagedResponse GetUsers(UsersToSelectPaginationParameters usersToSelectPaginationParameters)
        {
            return _userRepository.SelectUserPagedReponse(usersToSelectPaginationParameters);
        }

        public async Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllUsersAsync(EUserRole userRole)
        {
            return (await _userRepository.SelectAllUsersAsync(userRole))
                .Select(u => new UserShrinkedDetailsToSelectDTO()
                    {
                        Id = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }
                );
        }

        public async Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllPupilsAsync(int classId)
        {
            return (await _userRepository.SelectAllPupilsAsync(classId))
                .Select(u => new UserShrinkedDetailsToSelectDTO()
                    {
                        Id = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }
                );
        }

        public async Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllChildrenAsync(int userId)
        {
            return (await _userRepository.SelectAllChildrenAsync(userId))
                .Select(u => new UserShrinkedDetailsToSelectDTO()
                    {
                        Id = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }
                );
        }

        public async Task<IEnumerable<TeacherDetailsToSelectDTO>> SelectAllTeachersAsync(int classId)
        {
            return (await _userRepository.SelectAllTeachersAsync(classId))
                .Where(user => user.Teacher!.ClassesSubjectsTeachers.Any())
                .Select(user => new TeacherDetailsToSelectDTO()
                {
                    Id = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ContactEmail = user.Teacher!.ContactEmail,
                    ContactNumber = user.Teacher!.ContactNumber,
                    SubjectsTaught = user.Teacher.ClassesSubjectsTeachers.Select(classTeacherSubject => new SubjectDetailsToSelectDTO()
                        {
                            Id = classTeacherSubject.SubjectId,
                            Name = classTeacherSubject.Subject.Name
                        }
                    )
                }
            );
        }

        public async Task UpdateTeacherContactDetailsAsync(TeacherContactDetailsToUpdateDTO teacherContactDetailsToUpdateDTO)
        {
            await _userRepository.UpdateTeacherContactDetailsAsync(teacherContactDetailsToUpdateDTO.Id,
                                                                   teacherContactDetailsToUpdateDTO.ContactNumber,
                                                                   teacherContactDetailsToUpdateDTO.ContactEmail);
        }

        public async Task UpdateUserAccountActivityStatusAsync(UserAccountActivityToUpdateDTO userAccountActivityToUpdateDTO)
        {
            User? user = await _userRepository.UpdateUserAccountActivityStatusAsync(userAccountActivityToUpdateDTO.Id, userAccountActivityToUpdateDTO.IsActive);
            if (!userAccountActivityToUpdateDTO.IsActive && user != null)
            {
                StringBuilder emailBodyBuilder = new StringBuilder();
                emailBodyBuilder.Append($"Witaj {user.FirstName} {user.LastName},\n\n");
                emailBodyBuilder.Append("Twoje konto w aplikacji dziennik elektroniczny zostało dezaktywowane przez administratora. Od tej pory nie będziesz mógł się na nie logować.");
                await _eMailService.sendEMailAsync("Powiadomienie z aplikacji dziennik elektroniczny", user.Email, emailBodyBuilder.ToString());
            }
        }
    }
}
