using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Responses;
using VotePlatform.ViewModels.Users;

namespace VotePlatform.ViewModels.Organizations
{
    public class PeopleInOrganization
    {
        public string typeOfMembers;
        public bool isIncludeAdminPossible;
        public bool isExcludeAdminPossible;
        public bool isAcceptPersonPossible;
        public bool isExcludePersonPossible;
        public bool isRejectApplicationOfPersonPossible;
        public List<DemoUser> members;
        public PeopleInOrganization(string typeOfMembers, List<string> members, bool isIncludeAdminPossible, bool isExcludeAdminPossible, bool isAcceptPersonPossible, bool isExcludePersonPossible, bool isRejectApplicationOfPersonPossible)
        {
            this.typeOfMembers = typeOfMembers;
            this.isIncludeAdminPossible = isIncludeAdminPossible;
            this.isExcludeAdminPossible = isExcludeAdminPossible;
            this.isAcceptPersonPossible = isAcceptPersonPossible;
            this.isExcludePersonPossible = isExcludePersonPossible;
            this.isRejectApplicationOfPersonPossible = isRejectApplicationOfPersonPossible;

            this.members = new List<DemoUser>();
            foreach(var member in members) 
            {
                UsersDataBaseAPI.FindById(member, out User user);
                this.members.Add(new DemoUser(new UserDemoResponse(user)));
            }
        }
    }
}
