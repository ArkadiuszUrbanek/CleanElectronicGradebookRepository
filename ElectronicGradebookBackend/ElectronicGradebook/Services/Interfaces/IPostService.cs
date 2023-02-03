using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IPostService
    {
        Task<int> InsertPostAsync(PostDetailsToInsertDTO postDetailsToInsertDTO);
        Task UpdatePostAsync(PostDetailsToUpdateDTO postDetailsToUpdateDTO);
        Task DeletePostByIdAsync(int id);
        PostPagedResponse SelectPosts(BasePaginationParameters<EPostSortableProperties> basePaginationParameters, EUserRole userRole, int userId);
        Task InsertPostReactionAsync(PostReactionDetailsToInsertDTO postReactionDetailsToInsertDTO, int userId);
        Task UpdatePostReactionAsync(PostReactionDetailsToUpdateDTO postReactionDetailsToUpdateDTO, int userId);
        Task DeletePostReactionAsync(int postId, int userId);
    }
}
