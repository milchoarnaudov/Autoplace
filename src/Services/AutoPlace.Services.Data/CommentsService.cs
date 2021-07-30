namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Comments;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<int> CreateAsync(CreateComment comment)
        {
            if (comment is null ||
                comment.CommentatorId is null ||
                comment.CommentedUserId is null ||
                comment.Content is null)
            {
                return 0;
            }

            var commentEntity = new Comment
            {
                CommentatorId = comment.CommentatorId,
                CommentedUserId = comment.CommentedUserId,
                Content = comment.Content,
            };

            await this.commentRepository.AddAsync(commentEntity);
            await this.commentRepository.SaveChangesAsync();

            return commentEntity.Id;
        }
    }
}
