using Autoplace.Common.Data;
using Microsoft.AspNetCore.Identity;

namespace Autoplace.Identity.Data.Models
{
    public class User : IdentityUser, IAuditInfo, IDeletable
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
