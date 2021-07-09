namespace AutoPlace.Services.Data.Models.Comments
{
    public class CreateComment
    {
        public string CommentatorId { get; set; }

        public string Content { get; set; }

        public string CommentedUserId { get; set; }
    }
}
