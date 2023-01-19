using System.Collections.Generic;
using VoteM.Models.Votes.Serializable;

namespace VoteM.Models.Votes
{
    public class Voice
    {
        public string UserId { get; }
        public List<int> AnswerIndexes { get; }
        public DateTime VoteTime { get; }
        public Voice(string userId, List<int> answerIndexes, DateTime voteTime)
        {
            UserId = userId;
            AnswerIndexes = answerIndexes;
            VoteTime = voteTime;
        }
        public Voice(SVoiceV1 sVoice)
        {
            UserId = sVoice.UserId;
            AnswerIndexes = sVoice.AnswerIndexes;
            VoteTime = sVoice.VoteTime;
        }
    }
}
