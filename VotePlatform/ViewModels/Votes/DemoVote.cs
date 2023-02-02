using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.Votes;
using VotePlatform.ViewModels.Organizations;

namespace VotePlatform.ViewModels.Votes
{
    public class DemoVote
    {
        public bool IsAvailable;
        public string TimeCreated;
        public string title;
        public string description;
        public string urlToVote;
        public DemoOrganization ownerOrg; 

        public DemoVote(VoteDemoResponse response)
        {
            IsAvailable=response.IsAvailable;
            TimeCreated=response.CreatingDateTime.ToLongDateString();
            title = response.Meta.Header;
            description = response.Meta.Description;
            urlToVote = @$"{VRoutes.Controller}{VRoutes.AVoting}?id={response.Id.Id}";
            OrganizationsDataBaseAPI.FindById(response.Id.OwnerGroupId, out Organization organization);
            ownerOrg=new DemoOrganization(new OrganizationDemoResponse(organization));
        }
    }
}
