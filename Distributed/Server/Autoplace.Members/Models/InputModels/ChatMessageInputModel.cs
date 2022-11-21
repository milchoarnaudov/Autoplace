using Autoplace.Members.Common;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Members.Models.InputModels
{
    public class ChatMessageInputModel
    {
        [Required]
        public int? ChatId { get; set; }

        [Required]
        [MaxLength(Constants.MaxChatMessageContentLength)]
        [MinLength(Constants.MinChatMessageContentLength)]
        public string Content { get; set; }
    }
}
