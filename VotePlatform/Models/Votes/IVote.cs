using System.Collections.Generic;

namespace VoteM.Models.Votes
{
    interface IVote
    {
        public bool Voiting(string userId, List<int> choice);
        public bool Extend(string adminId, DateTime newDateTime);
        public bool Pin(string adminId);
        public bool Unpin(string adminId);
    }
}
