using Autoplace.Common.Mappings;
using Autoplace.Members.Data.Models;

namespace Autoplace.Members.Models.OutputModels
{
    public class ChatOutputModel : IMapFrom<Chat>
    {
        public int Id { get; set; }
    }
}
