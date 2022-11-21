using Autoplace.Common.Data.Models;
using Autoplace.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Administration.Data.Models
{
    public class ApprovalRequest : BaseEntity<int>
    {
        public int AutopartId { get; set; }

        public AutopartStatus Status { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<Image> Images { get; set; }
    }
}
