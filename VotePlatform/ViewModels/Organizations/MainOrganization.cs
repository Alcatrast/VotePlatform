using System.Collections.Generic;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.SystemServices;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.ViewModels.Organizations
{
    public class MainOrganization
    {
        public bool IsDeleted;

        public Meta meta;
        public OrganizationAttributes attributes;
        
        public int countMembers;
        public string urlToMembers;
        public List<DemoVote> votes;

        public MainOrganization(OrganizationMainResponse response)
        {
            IsDeleted= response.IsDeleted;
            meta = response.Meta;
            attributes=response.Attributes;
            countMembers = response.CountMembers;

            urlToMembers=string.Empty;

            votes=new List<DemoVote>();
            foreach (var vote in response.Votes) {votes.Add(new DemoVote(vote));}
        }
        
    }
}
