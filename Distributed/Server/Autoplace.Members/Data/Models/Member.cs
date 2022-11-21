using Autoplace.Common.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Members.Data.Models
{
    public class Member : BaseDeletableEntity<int>
    {
        public Member()
        {
            CommentsForMember = new HashSet<Comment>();
            CommentsByMember = new HashSet<Comment>();
            Chats = new HashSet<Chat>();
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ICollection<Comment> CommentsForMember { get; set; }

        public virtual ICollection<Comment> CommentsByMember { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
