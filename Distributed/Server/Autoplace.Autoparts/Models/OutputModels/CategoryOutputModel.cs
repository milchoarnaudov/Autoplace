using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class CategoryOutputModel : IMapFrom<AutopartCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}