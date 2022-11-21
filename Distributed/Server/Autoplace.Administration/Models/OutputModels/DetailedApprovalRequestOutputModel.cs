using Autoplace.Administration.Data.Models;
using Autoplace.Common.Enums;
using Autoplace.Common.Mappings;

namespace Autoplace.Administration.Models.OutputModels
{
    public class DetailedApprovalRequestOutputModel : ApprovalRequestOutputModel, IMapFrom<ApprovalRequest>
    {
        public string Status { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<ImageOutputModel> Images {get;set;}
}
}
