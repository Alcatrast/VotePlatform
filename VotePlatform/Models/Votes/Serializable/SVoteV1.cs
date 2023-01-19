using System;
using System.Collections.Generic;

using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Votes.Serializable
{
    [Serializable]
    public class SVoiceV1
    {
        public string UserId = string.Empty;
        public List<int> AnswerIndexes = new List<int>();
        public DateTime VoteTime = new DateTime();
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
        public TimeSpan TimeActiveToVote = new TimeSpan();
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
        public DateTime CreatingDateTime = new DateTime();
        public SVoteIdV1 Id = new SVoteIdV1();
        public VoteType Type = VoteType.AloneAswer;
        public SVoteAttributesV1 Attibutes = new SVoteAttributesV1();
        public SVoteResultAttributesV1 ResultAttributes = new SVoteResultAttributesV1();
        public SVoteMetaV1 Meta = new SVoteMetaV1();
        public List<SVoteMetaV1> AnswersMetas = new List<SVoteMetaV1>();
        public int CountVoters = 0;
        public List<SVoiceV1> Voices = new List<SVoiceV1>();
        public SVoteV1(Vote vote)
        {
            CreatingDateTime = vote.CreatingDateTime;
            Id = new SVoteIdV1(vote.Id);
            Type = vote.Type;
            Attibutes = new SVoteAttributesV1(vote.Attibutes);
            ResultAttributes = new SVoteResultAttributesV1(vote.ResultAttributes);
            Meta = new SVoteMetaV1(vote.Meta);
            CountVoters = vote.CountVoters;
            IsAvailable = vote.IsAvailable;

            AnswersMetas = new List<SVoteMetaV1>();
            foreach (var it in vote.AnswersMetas) { AnswersMetas.Add(new SVoteMetaV1(it)); }
            Voices = new List<SVoiceV1>();
            foreach (var it in vote.Voices) { Voices.Add(new SVoiceV1(it)); }
        }
        public SVoteV1() { }
    }
}
