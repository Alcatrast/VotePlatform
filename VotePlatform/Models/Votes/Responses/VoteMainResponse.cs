using System;
using System.Collections.Generic;
using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Votes
{
    public class VoteMainResponse
    {
        public bool IsAvailable { get; }
        
        public DateTime CreatingDateTime { get; }
        public VoteId Id { get; }
        public VoteType Type { get; }
        public VoteAttributes Attributes { get; }
        public VoteResultAttributes ResultAttributes { get; }
        public VoteMeta Meta { get; }
        public List<VoteMeta> AnswersMetas { get; }

        public List<int> UrerVoice { get; }
        public List<int> SimpleResults { get; private set; }

        public bool IsVotingAccessible { get; }
        public bool IsCancellationAccessible { get; }

        public bool IsActualResultAccessible { get; }
        public bool IsDynamicResultAccessible { get; }
        public int CountVoters { get; }
        public bool IsRoot { get; }

        public VoteMainResponse(Vote vote, string userId)
        {
            IsAvailable = vote.IsAvailable;
            Id = vote.Id;
            CreatingDateTime = vote.CreatingDateTime;
            Type = vote.Type;
            Attributes = vote.Attibutes;
            ResultAttributes = vote.ResultAttributes;
            Meta = vote.Meta;
            AnswersMetas = vote.AnswersMetas;
            CountVoters = vote.CountVoters;
            UrerVoice = GetUserVoice(vote, userId);
            SimpleResults = GetSimpleResults(vote, userId);
            IsVotingAccessible = vote.IsVotingAccessible(userId);
            IsCancellationAccessible=vote.IsCancellationPossible(userId);
            IsActualResultAccessible = GetIsActualResultAccessible(vote, userId);
            IsDynamicResultAccessible = GetIsDynamicResultAccessible(vote, userId);
            IsRoot= vote.IsAccessAllowed(userId, RoleInOrganization.Admin);
        }
        private List<int> GetSimpleResults(Vote vote, string userId)
        {
            List<int> res = new List<int>();
            for(int i=0;i< vote.AnswersMetas.Count; i++) { res.Add(0); }
            if (vote.IsAccessAllowed(userId, vote.ResultAttributes.MinRoleToActual) == false) { return res; }

            List<string> ids = new List<string>();

            for (int i = vote.Voices.Count - 1; i >= 0; i--)
            {
                if (ids.Contains(vote.Voices[i].UserId)) { continue; }
                else
                {
                    for(int gg=0; gg < res.Count; gg++)
                    {
                        res[gg] += vote.Voices[i].Answer[gg];
                    }
                    ids.Add(vote.Voices[i].UserId);
                }
            }
            return res;
        }
        private static bool GetIsActualResultAccessible(Vote vote, string userId) { return vote.IsAccessAllowed(userId, vote.ResultAttributes.MinRoleToActual); }/// }
        private static bool GetIsDynamicResultAccessible(Vote vote, string userId) { return vote.IsAccessAllowed(userId, vote.ResultAttributes.MinRoleToDynamic); }/// }
        private static List<int> GetUserVoice(Vote vote, string userId) 
        {
            List<int> res = new List<int>();
            for(int i=0;i<vote.AnswersMetas.Count;i++) { res.Add(0); }

            for (int i = vote.Voices.Count - 1; i >= 0; i--)
            {
                if (vote.Voices[i].UserId == userId)
                {
                    res = vote.Voices[i].Answer;
                    break;
                }
            }
            return res;
        }
    }
}
