using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<int> InsertPostAsync(PostDetailsToInsertDTO postDetailsToInsertDTO)
        {
            Post post = new Post()
            {
                UserId = postDetailsToInsertDTO.AuthorId,
                Contents = postDetailsToInsertDTO.Contents,
                CreationDate = DateTime.UtcNow
            };

            postDetailsToInsertDTO.AuthorizedRoles.Add(EUserRole.Admin);

            foreach (EUserRole authorizedRole in postDetailsToInsertDTO.AuthorizedRoles)
            {
                post.PostRoles.Add(new PostRole() { Role = authorizedRole });
            }

            return await _postRepository.InsertPostAsync(post);
        }

        public async Task UpdatePostAsync(PostDetailsToUpdateDTO postDetailsToUpdateDTO)
        {
            await _postRepository.UpdatePostAsync(postDetailsToUpdateDTO.Id,
                                                  postDetailsToUpdateDTO.Contents,
                                                  postDetailsToUpdateDTO.AuthorizedRoles);
        }

        public async Task DeletePostByIdAsync(int id)
        {
            await _postRepository.DeletePostByIdAsync(id);
        }

        public PostPagedResponse SelectPosts(BasePaginationParameters<EPostSortableProperties> basePaginationParameters, EUserRole userRole, int userId)
        {
            switch (userRole)
            {
                case EUserRole.Pupil:
                case EUserRole.Parent:
                    return _postRepository.SelectPostPagedReponse(basePaginationParameters, post => post.PostRoles.Contains(
                        new PostRole()
                        {
                            PostId = post.PostId,
                            Role = userRole
                        }),
                        userId);

                case EUserRole.Teacher:
                    return _postRepository.SelectPostPagedReponse(basePaginationParameters, post => post.UserId == userId || post.PostRoles.Contains(
                        new PostRole()
                        {
                            PostId = post.PostId,
                            Role = EUserRole.Teacher
                        }),
                        userId);

                case EUserRole.Admin:
                    return _postRepository.SelectPostPagedReponse(basePaginationParameters, s => true, userId);

                default: return null;
            }
        }

        public async Task InsertPostReactionAsync(PostReactionDetailsToInsertDTO postReactionDetailsToInsertDTO, int userId)
        {
            await _postRepository.InsertPostReactionAsync(postReactionDetailsToInsertDTO.Id, postReactionDetailsToInsertDTO.Type, userId);
        }

        public async Task UpdatePostReactionAsync(PostReactionDetailsToUpdateDTO postReactionDetailsToUpdateDTO, int userId)
        {
            await _postRepository.UpdatePostReactionAsync(postReactionDetailsToUpdateDTO.Id, postReactionDetailsToUpdateDTO.Type, userId);
        }

        public async Task DeletePostReactionAsync(int postId, int userId)
        {
            await _postRepository.DeletePostReactionAsync(postId, userId);
        }
    }
}
