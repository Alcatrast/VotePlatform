
using System.Collections.Generic;

using VoteM.Models.SystemServices;
using VoteM.Models.SystemServices.Serializable;

namespace VoteM.Models.Users.Serializable
{
    [Serializable]
    public class SMembersipImGroupsV1
    {
        public List<string> OwnerInGroups = new();
        public List<string> AdminInGroups = new();
        public List<string> AudienceInGtoups = new();
        public SMembersipImGroupsV1(MembersipImGroups membersipImGroups)
        {
            OwnerInGroups = membersipImGroups.OwnerInGroups;
            AdminInGroups = membersipImGroups.AdminInGroups;
            AudienceInGtoups = membersipImGroups.AudienceInGtoups;
        }
        public SMembersipImGroupsV1() { }
    }
    [Serializable]
    public class SUserV1
    {
        public string Id = string.Empty;
        public string Nickname = string.Empty;
        public string Email = string.Empty;
        public string Password = string.Empty;
        public SMetaV1 Meta = new();
        public RoleInPlatform Role = RoleInPlatform.Passerby;
        public SMembersipImGroupsV1 Membersip = new();
        public SUserV1(User user)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            Email = user.Email;
            Password = user.Password;
            Role = user.Role;
            Meta = new(user.Meta);
            Membersip = new(user.Membersip);
        }
        public SUserV1() { }
    }

}
