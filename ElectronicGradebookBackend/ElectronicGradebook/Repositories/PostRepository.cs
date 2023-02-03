using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public PostRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<int> InsertPostAsync(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return post.PostId;
        }

        public async Task UpdatePostAsync(int id, string? contents, HashSet<EUserRole>? authorizedRoles)
        {
            var foundPost = await _dbContext.Posts
                .Where(post => post.PostId == id)
                .Include(post => post.PostRoles)
                .SingleOrDefaultAsync();

            if (foundPost != null)
            {
                foundPost.Contents = contents ?? foundPost.Contents;

                if (authorizedRoles != null)
                {
                    authorizedRoles.Add(EUserRole.Admin);
                    var currentlyAuthorizedRoles = foundPost.PostRoles.Select(pr => pr.Role).ToHashSet();

                    var rolesToDeauthorize = currentlyAuthorizedRoles.Except(authorizedRoles);
                    if (rolesToDeauthorize.Any()) _dbContext.PostRoles.RemoveRange(_dbContext.PostRoles.Where(pr => pr.PostId == foundPost.PostId && rolesToDeauthorize.Contains(pr.Role)));

                    var rolesToAuthorize = authorizedRoles.Except(currentlyAuthorizedRoles);
                    if (rolesToAuthorize.Any()) _dbContext.PostRoles.AddRange(rolesToAuthorize.Select(r => new PostRole()
                    {
                        PostId = foundPost.PostId,
                        Role = r
                    }));
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeletePostByIdAsync(int id)
        {
            _dbContext.Posts.Remove(new Post { PostId = id });
            await _dbContext.SaveChangesAsync();
        }

        public PostPagedResponse SelectPostPagedReponse(BasePaginationParameters<EPostSortableProperties> basePaginationParameters, Expression<Func<Post, bool>> predicate, int userId)
        {
            return new PostPagedResponse(
                _dbContext.Posts
                .Where(predicate)
                .Include(post => post.User)
                .Include(post => post.PostRoles)
                .Include(post => post.PostReactions),
                basePaginationParameters.PageNumber,
                basePaginationParameters.PageSize,
                basePaginationParameters.SortBy,
                basePaginationParameters.Order,
                userId
           );
        }

        public async Task InsertPostReactionAsync(int id, EPostReaction type, int userId)
        {
            _dbContext.PostReactions.Add(new PostReaction() { PostId = id, Type = type, UserId = userId });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePostReactionAsync(int id, EPostReaction type, int userId)
        {
            var foundPostReaction = await _dbContext.PostReactions.SingleOrDefaultAsync(postReaction => postReaction.PostId == id && postReaction.UserId == userId);
            if (foundPostReaction != null)
            {
                foundPostReaction.Type = type;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeletePostReactionAsync(int postId, int userId)
        {
            PostReaction? postReaction = await _dbContext.PostReactions.SingleOrDefaultAsync(postReaction => postReaction.PostId == postId && postReaction.UserId == userId);

            if (postReaction != null)
            {
                _dbContext.PostReactions.Remove(postReaction);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
