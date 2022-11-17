using Autoplace.Common.Data;

namespace Autoplace.Members.Data
{
    public class Member : BaseDeletableModel<int>
    {
        public Member()
        {
            this.CommentsForUser = new HashSet<Comment>();
            this.CommentsByUser = new HashSet<Comment>();
            this.MessagesReceived = new HashSet<Message>();
            this.MessagesSent = new HashSet<Message>();
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Comment> CommentsForUser { get; set; }

        public virtual ICollection<Comment> CommentsByUser { get; set; }

        public virtual ICollection<Message> MessagesReceived { get; set; }

        public virtual ICollection<Message> MessagesSent { get; set; }
    }
}
