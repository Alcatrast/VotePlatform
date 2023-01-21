using VotePlatform.Models.Organizations;

namespace VotePlatform.ViewModels.Organizations
{
    public class DemoOrganization
    {
        public string name;
        public string nick;
        public string url;
    
        public DemoOrganization(OrganizationDemoResponse response)
        {
            name = response.Meta.Name;
            nick = response.Nick;
            url = string.Empty;
        }
    }
}
