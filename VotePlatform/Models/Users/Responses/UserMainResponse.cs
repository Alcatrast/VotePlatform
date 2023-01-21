using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Users.Responses
{
    public class UserMainResponse
    {
        public string Id { get; }
        public string Nickname { get; }
        public string Email { get; }
        public Meta Meta { get; }
        public RoleInPlatform Role { get; }
        public MembersipImGroups Membersip { get; }

        public UserMainResponse(User user, string userId)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            if (user.Id == userId) { Email=user.Email; }
            else
            { UsersDataBaseAPI.FindById(userId, out User valid);
                if ((sbyte)valid.Role >= (sbyte)RoleInPlatform.Validator) { Email = user.Email; } else { Email = string.Empty; }
            }
            Meta = user.Meta;
            Role = user.Role;
            Membersip = user.Membersip;
        }
    }
}
