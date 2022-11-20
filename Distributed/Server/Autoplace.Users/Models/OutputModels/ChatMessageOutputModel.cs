using Autoplace.Common.Mappings;
using Autoplace.Members.Data.Models;

namespace Autoplace.Members.Models.OutputModels
{
    public class ChatMessageOutputModel : IMapFrom<ChatMessage>
    {
        public int Id { get; set; }

        public string Content { get; set; }
    }
}
