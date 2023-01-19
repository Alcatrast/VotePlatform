using System;
using System.Collections.Generic;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Votes.Serializable;

namespace VotePlatform.Models.Votes
{
    public class VoteAttributes
    {
        public RoleInOrganization MinRoleToVoting { get; }
        public TimeSpan TimeActiveToVote { get; set; }
        public bool IsAlwaysActiveToVote { get; }
        public bool IsAnonimousVote { get; }
        public bool IsVoiceCancellationPossible { get; }
        public bool IsExtendPossible { get; }
        public VoteAttributes(TimeSpan timeActiveVote, bool isExtendPossible, bool isAlwaysActiveToVote, bool anonimousVote, bool isVoiceCancellationPossible, RoleInOrganization minRoleToVoting)
        {
            TimeActiveToVote = timeActiveVote;
            IsAnonimousVote = anonimousVote;
            IsVoiceCancellationPossible = isVoiceCancellationPossible;
            MinRoleToVoting = minRoleToVoting;
            IsAlwaysActiveToVote = isAlwaysActiveToVote;
            IsExtendPossible = isExtendPossible;
        }
        public VoteAttributes(SVoteAttributesV1 sVoteAttributes)
        {
            MinRoleToVoting = sVoteAttributes.MinRoleToVoting;
            TimeActiveToVote = sVoteAttributes.TimeActiveToVote;
            IsAlwaysActiveToVote = sVoteAttributes.IsAlwaysActiveToVote;
            IsAnonimousVote = sVoteAttributes.IsAnonimousVote;
            IsVoiceCancellationPossible = sVoteAttributes.IsVoiceCancellationPossible;
            IsExtendPossible = sVoteAttributes.IsExtendPossible;
        }
    }
}
