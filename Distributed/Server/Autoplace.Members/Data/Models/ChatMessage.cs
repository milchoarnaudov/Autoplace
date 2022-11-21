using Autoplace.Common.Data.Models;
using Autoplace.Members.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autoplace.Members.Data.Models
{
    public class ChatMessage : BaseDeletableEntity<int>
    {
        [MaxLength(Constants.MaxChatMessageContentLength)]
        [Required]
        public string Content { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public int SenderId { get; set; }

        public Member Sender { get; set; }
    }
}
