using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Users.Responses
{
    public class UserDemoResponse
    {
        public string Id { get; }
        public string Nickname { get; }
        public string Name { get; }
        public UserDemoResponse(User user)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            Name = user.Meta.Name;
        }

    }
}
