using Autoplace.Common.Data.Models;
using Autoplace.Members.Common;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Members.Data.Models
{
    public class Chat : BaseDeletableEntity<int>
    {
        public Chat()
        {
            Members = new HashSet<Member>();
            ChatMessages = new HashSet<ChatMessage>();
        }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
