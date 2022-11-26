using Autoplace.Administration.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Administration.Models.OutputModels
{
    public class ApprovalRequestOutputModel : IMapFrom<ApprovalRequest>
    {
        public int Id { get; set; }

        public string AutopartId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
