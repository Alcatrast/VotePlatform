using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteM.Models.SystemServices;

namespace VoteM.Models.Users
{
    public interface IUser
    {
        public bool IsPasswordRight(string password);
        public bool ChangeRole(string givingUserId, RoleInPlatform newRole);

    }
}
