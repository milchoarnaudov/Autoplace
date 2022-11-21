using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class AutopartOutputModel : IMapFrom<Autopart>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public decimal Price { get; set; }
    }
}
