using Autoplace.Identity.Data.Models;

namespace Autoplace.Identity.Services
{
    public interface ITokenProviderService
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}
