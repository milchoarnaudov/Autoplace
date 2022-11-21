using Autoplace.Common.Data.Models;
using Autoplace.Members.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autoplace.Members.Data.Models
{
    public class Comment : BaseDeletableEntity<int>
    {
        [MaxLength(Constants.MaxCommentLength)]
        [Required]
        public string Content { get; set; }

        public int ByMemberId { get; set; }

        [InverseProperty("CommentsByMember")]
        public virtual Member ByMember { get; set; }

        public int ForMemberId { get; set; }

        [InverseProperty("CommentsForMember")]
        public virtual Member ForMember { get; set; }
    }
}
