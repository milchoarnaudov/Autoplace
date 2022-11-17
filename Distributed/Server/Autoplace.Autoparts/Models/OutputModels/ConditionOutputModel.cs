using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class ConditionOutputModel : IMapFrom<AutopartCondition>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}