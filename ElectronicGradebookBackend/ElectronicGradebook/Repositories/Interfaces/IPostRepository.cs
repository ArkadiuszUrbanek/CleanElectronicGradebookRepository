using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<int> InsertPostAsync(Post post);
        Task UpdatePostAsync(int id, string? contents, HashSet<EUserRole>? authorizedRoles);
        Task DeletePostByIdAsync(int id);
        PostPagedResponse SelectPostPagedReponse(BasePaginationParameters<EPostSortableProperties> basePaginationParameters, Expression<Func<Post, bool>> predicate, int userId);
        Task InsertPostReactionAsync(int id, EPostReaction type, int userId);
        Task UpdatePostReactionAsync(int id, EPostReaction type, int userId);
        Task DeletePostReactionAsync(int postId, int userId);
    }
}
