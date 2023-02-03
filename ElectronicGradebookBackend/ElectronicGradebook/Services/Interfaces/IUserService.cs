using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(UserCreateDTO userCreateDTO);
        UserPagedResponse GetUsers(UsersToSelectPaginationParameters usersToSelectPaginationParameters);
        Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllUsersAsync(EUserRole userRole);
        Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllPupilsAsync(int classId);
        Task<IEnumerable<UserShrinkedDetailsToSelectDTO>> SelectAllChildrenAsync(int userId);
        Task<IEnumerable<TeacherDetailsToSelectDTO>> SelectAllTeachersAsync(int classId);
        Task UpdateTeacherContactDetailsAsync(TeacherContactDetailsToUpdateDTO teacherContactDetailsToUpdateDTO);
        Task UpdateUserAccountActivityStatusAsync(UserAccountActivityToUpdateDTO userAccountActivityToUpdateDTO);
    }
}
