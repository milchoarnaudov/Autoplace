using Autoplace.Common.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autoplace.Members.Data
{
    public class Comment : BaseDeletableModel<int>
    {
        [MaxLength(300)]
        [Required]
        public string Content { get; set; }

        public string CommentedByUserId { get; set; }

        [InverseProperty("CommentsByUser")]
        public virtual Member CommentedByUser { get; set; }

        public string CommentedUserId { get; set; }

        [InverseProperty("CommentsForUser")]
        public virtual Member CommentedUser { get; set; }
    }
}
