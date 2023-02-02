using System;
using System.Collections.Generic;

namespace VotePlatform.Models.Votes
{
    interface IVote
    {
        public bool Voting(string userId, List<int> choice);
        public bool Extend(string adminId, DateTime newDateTime);
        public bool Pin(string adminId);
        public bool Unpin(string adminId);
    }
}
