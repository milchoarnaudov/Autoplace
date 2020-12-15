namespace AutoPlace.Services.Data.DTO.Comments
{
    public class CreateCommentDTO
    {
        public string CommentatorId { get; set; }

        public string Content { get; set; }

        public string CommentedUserId { get; set; }
    }
}
