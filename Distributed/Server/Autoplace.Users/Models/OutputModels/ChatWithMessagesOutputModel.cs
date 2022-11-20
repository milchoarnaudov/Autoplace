using Autoplace.Common.Mappings;
using Autoplace.Members.Data.Models;

namespace Autoplace.Members.Models.OutputModels
{
    public class ChatWithMessagesOutputModel : ChatOutputModel, IMapFrom<Chat>
    {
        public IEnumerable<ChatMessageWithSenderOutputModel> ChatMessages { get; set; }
    }
}
