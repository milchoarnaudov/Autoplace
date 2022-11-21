using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class AutopartOutputModel : BaseAutopartOutputModel, IMapFrom<Autopart>
    {
        public decimal Price { get; set; }

        public string Description { get; set; }

        public int ViewsCount { get; set; }

        public string Username { get; set; }

        public CategoryOutputModel Category { get; set; }

        public ConditionOutputModel Condition { get; set; }

        public CarOutputModel Car { get; set; }
    }
}
