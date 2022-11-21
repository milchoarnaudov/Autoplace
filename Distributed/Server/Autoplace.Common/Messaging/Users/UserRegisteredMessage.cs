using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Messaging.Users
{
    public class UserRegisteredMessage
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
    }
}
