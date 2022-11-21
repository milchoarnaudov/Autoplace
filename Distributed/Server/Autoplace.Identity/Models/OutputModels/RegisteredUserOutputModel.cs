using Autoplace.Common.Mappings;
using Autoplace.Identity.Data.Models;

namespace Autoplace.Identity.Models.OutputModels
{
    public class RegisteredUserOutputModel : IMapFrom<User>
    {
        public string Username { get; set; }

        public string Email { get; set; }
    }
}
