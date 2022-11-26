using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{
    public class Image : BaseEntity<string>
    {
        public Image()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public string AutopartId { get; set; }

        public virtual Autopart Autopart { get; set; }
    }
}
