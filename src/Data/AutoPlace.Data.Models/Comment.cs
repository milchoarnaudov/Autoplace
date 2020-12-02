namespace AutoPlace.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoPlace.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public string CommentatorId { get; set; }

        [InverseProperty("CommentsByUser")]
        public virtual ApplicationUser Commentator { get; set; }

        public string CommentedUserId { get; set; }

        [InverseProperty("CommentsForUser")]
        public virtual ApplicationUser CommentedUser { get; set; }
    }
}
