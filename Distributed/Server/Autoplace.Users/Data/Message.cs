using Autoplace.Common.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autoplace.Members.Data
{
    public class Message : BaseDeletableModel<int>
    {
        [MaxLength(100)]
        [Required]
        public string Topic { get; set; }

        [MaxLength(500)]
        [Required]
        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public string AutopartId { get; set; }

        public string AutopartName { get; set; }

        public string ReceiverId { get; set; }

        [InverseProperty("MessagesReceived")]
        public Member Receiver { get; set; }

        public string SenderId { get; set; }

        [InverseProperty("MessagesSent")]
        public Member Sender { get; set; }
    }
}
