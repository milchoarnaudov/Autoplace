using Autoplace.Common.Data.Models;

namespace Autoplace.Administration.Data.Models
{
    public class Image : BaseEntity<int>
    {
        public string FileId { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public int ApprovalRequestId { get; set; }

        public ApprovalRequest ApprovalRequest { get; set; }
    }
}
