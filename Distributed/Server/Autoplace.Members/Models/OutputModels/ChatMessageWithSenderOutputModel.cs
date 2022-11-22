using Autoplace.Common.Mappings;
using Autoplace.Members.Data.Models;

namespace Autoplace.Members.Models.OutputModels
{
    public class ChatMessageWithSenderOutputModel : ChatMessageOutputModel, IMapFrom<ChatMessage>
    {
        public MemberOutputModel Sender { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
