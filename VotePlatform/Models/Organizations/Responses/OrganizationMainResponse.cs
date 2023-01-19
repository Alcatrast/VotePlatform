using System.Collections.Generic;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Votes;
using VotePlatform.Models.DataBaseAPI;

namespace VotePlatform.Models.Organizations
{
    public class OrganizationMainResponse
    {
        public bool IsDeleted { get; set; }

        public string Id { get; private set; }
        public Meta Meta { get; private set; }
        public OrganizationAttributes Attributes { get; private set; }
        public int CountMembers { get; private set; }
        public List<VoteMainResponse> Votes { get; private set; }
        public string UserId { get; private set; }
        public RoleInOrganization InGroupRole { get; private set; }
        public RoleInPlatform InPlatformRole { get; private set; }
        public OrganizationMainResponse(Organization group, string userId)
        {
            IsDeleted = group.IsDeleted;
            Id = group.Id;
            Meta = group.Meta;
            Attributes = group.Attributes;
            CountMembers = group.Members.AllMembers.Count;
            UserId = userId;
            InGroupRole = group.GetRoleInOrganization(userId);
            InPlatformRole = group.GetRoleInPlatform(userId);
            Votes = GetVotes(group, userId);
        }
        private static List<VoteMainResponse> GetVotes(Organization group, string userId)
        {
            List<VoteMainResponse> res = new List<VoteMainResponse>(), pvs = new List<VoteMainResponse>(), upvs = new List<VoteMainResponse>();

            var votes = VotesDataBaseAPI.FindAllBelongingTo(group, group.GetRoleInOrganization(userId), group.GetRoleInPlatform(userId));

            foreach (var vote in votes)
            {
                var mrv = new VoteMainResponse(vote, userId);
                if (mrv.Meta.IsPinned) { pvs.Add(mrv); }
                else { upvs.Add(mrv); }
            }

            pvs.Sort(); pvs.Reverse();
            upvs.Sort(); upvs.Reverse();

            foreach (var it in pvs) { res.Add(it); }
            foreach (var it in upvs) { res.Add(it); }
            return res;
        }
    }
}
