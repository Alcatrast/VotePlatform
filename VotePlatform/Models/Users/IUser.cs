using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Users
{
    public interface IUser
    {
        public bool IsPasswordRight(string password);
        public bool ChangeRole(string givingUserId, RoleInPlatform newRole);

    }
}
