
using System.Collections.Generic;

using VoteM.Models.SystemServices;

namespace VoteM.Models.Votes.Serializable
{
    [Serializable]
    public class SVoiceV1
    {
        public string UserId = string.Empty;
        public List<int> AnswerIndexes = new();
        public DateTime VoteTime = new();
        public SVoiceV1(Voice voice)
        {
            UserId = voice.UserId;
            AnswerIndexes = voice.AnswerIndexes;
            VoteTime = voice.VoteTime;
        }
        public SVoiceV1() { }
    }
    [Serializable]
    public class SVoteResultAttributesV1
    {
        public bool ResultsOnlyAfterCompletion = false;
        public RoleInOrganization MinRoleToActual = RoleInOrganization.Passerby;
        public RoleInOrganization MinRoleToDynamic = RoleInOrganization.Passerby;

        public SVoteResultAttributesV1(VoteResultAttributes voteResultAttributes)
        {
            ResultsOnlyAfterCompletion = voteResultAttributes.ResultsOnlyAfterCompletion;
            MinRoleToActual = voteResultAttributes.MinRoleToActual;
            MinRoleToDynamic = voteResultAttributes.MinRoleToDynamic;
        }
        public SVoteResultAttributesV1() { }
    }
    [Serializable]
    public class SVoteMetaV1
    {
        public string Header = string.Empty;
        public bool IsPinned = false;
        public string Description = string.Empty;
        public SVoteMetaV1(VoteMeta voteMeta)
        {
            Header = voteMeta.Header;
            IsPinned = voteMeta.IsPinned;
            Description = voteMeta.Description;
        }
        public SVoteMetaV1() { }
    }
    [Serializable]
    public class SVoteIdV1
    {
        public string OwnerGroupId = string.Empty;
        public string Index = string.Empty;
        public SVoteIdV1(VoteId voteId)
        {
            OwnerGroupId = voteId.OwnerGroupId;
            Index = voteId.IndexIn;
        }
        public SVoteIdV1() { }
    }
    [Serializable]
    public class SVoteAttributesV1
    {
        public RoleInOrganization MinRoleToVoting { get; }
        public TimeSpan TimeActiveToVote = new();
        public bool IsAlwaysActiveToVote = false;
        public bool IsAnonimousVote = false;
        public bool IsVoiceCancellationPossible = false;
        public bool IsExtendPossible = false;
        public SVoteAttributesV1(VoteAttributes voteAttributes)
        {
            TimeActiveToVote = voteAttributes.TimeActiveToVote;
            IsAnonimousVote = voteAttributes.IsAnonimousVote;
            IsVoiceCancellationPossible = voteAttributes.IsVoiceCancellationPossible;
            MinRoleToVoting = voteAttributes.MinRoleToVoting;
            IsAlwaysActiveToVote = voteAttributes.IsAlwaysActiveToVote;
            IsExtendPossible = voteAttributes.IsExtendPossible;
        }
        public SVoteAttributesV1() { }
    }
    [Serializable]
    public class SVoteV1
    {
        public bool IsAvailable = false;
        public DateTime CreatingDateTime = new();
        public SVoteIdV1 Id = new();
        public VoteType Type = VoteType.AloneAswer;
        public SVoteAttributesV1 Attibutes = new();
        public SVoteResultAttributesV1 ResultAttributes = new();
        public SVoteMetaV1 Meta = new();
        public List<SVoteMetaV1> AnswersMetas = new();
        public int CountVoters = 0;
        public List<SVoiceV1> Voices = new();
        public SVoteV1(Vote vote)
        {
            CreatingDateTime = vote.CreatingDateTime;
            Id = new(vote.Id);
            Type = vote.Type;
            Attibutes = new(vote.Attibutes);
            ResultAttributes = new(vote.ResultAttributes);
            Meta = new(vote.Meta);
            CountVoters = vote.CountVoters;
            IsAvailable = vote.IsAvailable;

            AnswersMetas = new();
            foreach (var it in vote.AnswersMetas) { AnswersMetas.Add(new(it)); }
            Voices = new();
            foreach (var it in vote.Voices) { Voices.Add(new(it)); }
        }
        public SVoteV1() { }
    }

}
