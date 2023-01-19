using System.Collections.Generic;
using VoteM.Models.Users.Serializable;

namespace VoteM.Models.Users
{
    public class MembersipImGroups
    {
        public List<string> OwnerInGroups { get; }
        public List<string> AdminInGroups { get; }
        public List<string> AudienceInGtoups { get; }
        public MembersipImGroups()
        {
            OwnerInGroups = new List<string>();
            AdminInGroups = new List<string>();
            AudienceInGtoups = new List<string>();
        }
        public MembersipImGroups(SMembersipImGroupsV1 sMembersipImGroups)
        {
            OwnerInGroups = sMembersipImGroups.OwnerInGroups;
            AdminInGroups = sMembersipImGroups.AdminInGroups;
            AudienceInGtoups = sMembersipImGroups.AudienceInGtoups;
        }
    }
}
