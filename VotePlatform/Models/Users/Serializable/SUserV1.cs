using System;
using System.Collections.Generic;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.SystemServices.Serializable;

namespace VotePlatform.Models.Users.Serializable
{
    [Serializable]
    public class SMembersipImGroupsV1
    {
        public List<string> OwnerInGroups = new List<string>();
        public List<string> AdminInGroups = new List<string>();
        public List<string> AudienceInGtoups = new List<string>();
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
        public SMetaV1 Meta = new SMetaV1();
        public RoleInPlatform Role = RoleInPlatform.Passerby;
        public SMembersipImGroupsV1 Membersip = new SMembersipImGroupsV1();
        public SUserV1(User user)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            Email = user.Email;
            Password = user.Password;
            Role = user.Role;
            Meta = new SMetaV1(user.Meta);
            Membersip = new SMembersipImGroupsV1(user.Membersip);
        }
        public SUserV1() { }
    }

}
