using Autoplace.Administration.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Administration.Models.OutputModels
{
    public class ImageOutputModel : IMapFrom<Image>
    {
        public string FileId { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }
    }
}