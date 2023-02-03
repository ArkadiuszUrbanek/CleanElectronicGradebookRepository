using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.DTOs
{
    public class PostPagedResponse : BasePagedResponse<PostDetailsToSelectDTO, Post, EPostSortableProperties> 
        
    {
        public PostPagedResponse(IQueryable<Post> source, int pageNumber, int pageSize, EPostSortableProperties orderBy, EOrder order, int userId) : base(source, pageNumber, pageSize, orderBy, order, userId)
        {
           
        }

        protected override IQueryable<Post> PerformSortingLogic(IQueryable<Post> source, EPostSortableProperties orderBy, EOrder order)
        {
            switch (order) {
                case EOrder.Ascending:
                    { 
                        switch (orderBy) {
                            case EPostSortableProperties.Id: return source.OrderBy(post => post.PostId);
                            case EPostSortableProperties.CreationDate: return source.OrderBy(post => post.CreationDate.Date).ThenBy(post => post.CreationDate.TimeOfDay);
                            default: return source;

                        }
                    }
                case EOrder.Descending:
                    {
                        switch (orderBy)
                        {
                            case EPostSortableProperties.Id: return source.OrderByDescending(post => post.PostId);
                            case EPostSortableProperties.CreationDate: return source.OrderByDescending(post => post.CreationDate.Date).ThenByDescending(post => post.CreationDate.TimeOfDay);
                            default: return source;
                        }
                    }
                default: return source;
            }
        }

        private static Dictionary<EPostReaction, int> MapPostReactions(Post post)
        {
            Dictionary<EPostReaction, int> usersReactions = new Dictionary<EPostReaction, int>();

            foreach (EPostReaction ePostReaction in Enum.GetValues<EPostReaction>())
            {
                usersReactions.Add(ePostReaction, post.PostReactions.Count(postReaction => postReaction.Type == ePostReaction));
            }

            return usersReactions;
        }

        private static EPostReaction? GetUserReaction(Post post, int userId) {
            var postReaction = post.PostReactions.SingleOrDefault(postReaction => postReaction.UserId == userId);
            return postReaction?.Type; 
        }

        protected override IQueryable<PostDetailsToSelectDTO> PerformMapping(IQueryable<Post> source, int? userId)
        {

            return source.Select(post => new PostDetailsToSelectDTO()
            {
                Id = post.PostId,
                Contents = post.Contents,
                CreationDate = post.CreationDate,
                Author = new UserDetailsToSelectDTO()
                {
                    Id = post.UserId,
                    FirstName = post.User.FirstName,
                    LastName = post.User.LastName,
                    Role = post.User.Role,
                    IsActive = post.User.IsActive
                },
                AuthorizedRoles = post.PostRoles.Select(postRole => postRole.Role).ToHashSet(),
                UsersReactions = MapPostReactions(post),
                CurrentUserReaction = GetUserReaction(post, (int)userId)
            });
        }
    }
}
