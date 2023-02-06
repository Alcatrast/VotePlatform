using System.Collections.Generic;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.SystemServices;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.ViewModels.Organizations
{
    public class MainOrganization
    {
        public bool IsDeleted;
        public string Id;
        public Meta meta;
        public OrganizationAttributes attributes;
        
        public int countMembers;
        public string urlToAudience;
        public string urlToAdmins;
        public string urlToQueue;
        public List<DemoVote> votes;
        public RoleInOrganization inGroupRole;
        public RoleInPlatform inPlatformRole;

        public MainOrganization(OrganizationMainResponse response)
        {
            Id = response.Id;
            IsDeleted= response.IsDeleted;
            meta = response.Meta;
            attributes=response.Attributes;
            countMembers = response.CountMembers;

            urlToAudience=$@"{ORoutes.Controller}{ORoutes.AAudience}?id={response.Id}";
            urlToAdmins = $@"{ORoutes.Controller}{ORoutes.AAdmins}?id={response.Id}";
            urlToQueue = $@"{ORoutes.Controller}{ORoutes.AQueue}?id={response.Id}";

            votes=new List<DemoVote>();
            foreach (var vote in response.Votes) {votes.Add(new DemoVote(vote));}
            inGroupRole=response.InGroupRole;
            inPlatformRole=response.InPlatformRole;
        }
        
    }
}
