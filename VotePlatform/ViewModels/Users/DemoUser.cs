using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Responses;

namespace VotePlatform.ViewModels.Users
{
    public class DemoUser
    {
        public string name;
        public string nick;
        public string url;

        public DemoUser(UserDemoResponse response)
        {
            name=response.Name;
            nick = response.Nickname;
            url = string.Empty;
        }
    }
}
