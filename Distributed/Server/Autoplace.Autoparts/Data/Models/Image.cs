using Autoplace.Common.Data;
using Autoplace.Autoparts.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{
    public class Image : BaseModel<string>
    {
        public Image()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public int AutopartId { get; set; }

        public virtual Autopart Autopart { get; set; }
    }
}
