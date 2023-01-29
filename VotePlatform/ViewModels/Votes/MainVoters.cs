using System.Collections.Generic;
using VotePlatform.ViewModels.Users;

namespace VotePlatform.ViewModels.Votes
{
    public class MainVoters
    {
        public List<DemoUser> voters;
        public List<int> values;
        public MainVoters(List<DemoUser> voters, List<int> values)
        {
            this.voters = voters;
            this.values = values;
        }
    }
}
