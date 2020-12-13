namespace AutoPlace.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoPlace.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        [MaxLength(100)]
        [Required]
        public string Topic { get; set; }

        [MaxLength(500)]
        [Required]
        public string Content { get; set; }

        public int? AutopartId { get; set; }

        public Autopart Autopart { get; set; }

        public string ReceiverId { get; set; }

        [InverseProperty("MessagesReceived")]
        public ApplicationUser Receiver { get; set; }

        public string SenderId { get; set; }

        [InverseProperty("MessagesSent")]
        public ApplicationUser Sender { get; set; }
    }
}
