using System.Collections.Generic;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Votes;
using VotePlatform.Models.DataBaseAPI;

namespace VotePlatform.Models.Organizations
{
    public class OrganizationDemoResponse
    {
        public bool IsDeleted { get; }
        public string Id { get; }
        public string Nick { get; }
        public Meta Meta { get; }
        public int CountMembers { get;}
        public OrganizationDemoResponse(Organization group)
        {
            IsDeleted = group.IsDeleted;
            Id = group.Id;
            Nick = group.Nickname;
            Meta = group.Meta;
            CountMembers=group.Members.AllMembers.Count;
        }
    }
}
