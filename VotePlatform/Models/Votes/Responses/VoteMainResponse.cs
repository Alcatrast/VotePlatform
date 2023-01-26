using Azure;
using System;
using System.Collections.Generic;

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
        public List<int> SimpleResults { get; }

        public bool IsVotingAccessible { get; }

        public bool IsActualResultAccessible { get; }
        public bool IsDynamicResultAccessible { get; }
        public int CountVoters { get; }

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
            IsActualResultAccessible = GetIsActualResultAccessible(vote, userId);
            IsDynamicResultAccessible = GetIsDynamicResultAccessible(vote, userId);
        }
        private List<int> GetSimpleResults(Vote vote, string userId)
        {
            List<int> res = new List<int>();
            for(int i=0;i< vote.AnswersMetas.Count; i++) { res.Add(0); }
            if (IsActualResultAccessible == false || vote.IsAccessAllowed(userId, vote.ResultAttributes.MinRoleToActual) == false) { return res; }

            List<string> ids = new List<string>();

            for (int i = vote.Voices.Count - 1; i >= 0; i--)
            {
                if (ids.Contains(vote.Voices[i].UserId)) { continue; }
                else
                {
                    if (vote.Voices[i].AnswerIndexes[0] > -1)
                    {
                        for (int index = 0; index < vote.Voices[i].AnswerIndexes.Count; index++)
                        {
                            if (Type == VoteType.PreferVote)
                            {
                                res[vote.Voices[i].AnswerIndexes[index]] += vote.AnswersMetas.Count - 1 - index;
                            }
                            else
                            {
                                res[vote.Voices[i].AnswerIndexes[index]]++;
                            }
                        }
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

            //for(int i = vote.Voices.Count - 1; i >= 0; i--)
            //{
            //    if (vote.Voices[i].UserId == userId)
            //    {
            //        res = vote.Voices[i].AnswerIndexes;
            //        break;
            //    }
            //}
            return res;
        }
    }
}
