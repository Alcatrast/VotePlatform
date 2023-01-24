﻿using System.Collections.Generic;

using VotePlatform.Models.Votes;
using VotePlatform.Models.SystemServices;

namespace VotePlatform.ViewModels.Votes
{

    public class MainVote
    {
        public bool isAvailable;
        public VoteMeta meta;
        public VoteAttributes attributes;
        public string creatingDateTime;
        public VoteType type;
        public string typeStr;
        public string minRoleToVoting;
        public string timeActiveToVote;
        public string isAnonimousVote;
        public string isVoiceCancellationPossible;
        public string isExtendPossible;
        public string resultsOnlyAfterCompletion;

        public string attributesStr;

        public bool isVotingAccessible;

        public List<VoteMeta> answersMetas;
        public List<int> userVoice;

        public bool isActualResultAccessible;
        public int countVoters;
        public List<int> simpleResults;

        public bool isDynamicResultAccessible;
        public string urlToDynamic;

        public MainVote(VoteMainResponse response)
        {
            isAvailable = response.IsAvailable;
            attributes= response.Attributes;
            meta = response.Meta;
            creatingDateTime = response.CreatingDateTime.ToLongDateString();
            type= response.Type;
            if (response.Type == VoteType.AloneAswer) { typeStr = ""; }
            if (response.Type == VoteType.SomeAnswers) { typeStr = "несколько вариантов"; }
            if (response.Type == VoteType.PreferVote) { typeStr = "предпочтение"; }

            if (response.Attributes.MinRoleToVoting >= RoleInOrganization.Audience) { minRoleToVoting = "частное голосование"; } else { minRoleToVoting = string.Empty; }

            if (response.Attributes.IsAlwaysActiveToVote) { timeActiveToVote = ""; }
            else { timeActiveToVote = (response.CreatingDateTime + response.Attributes.TimeActiveToVote).ToLongDateString(); }

            isAnonimousVote = response.Attributes.IsAnonimousVote ? "анонимное голосование" : string.Empty;
            isVoiceCancellationPossible = response.Attributes.IsAnonimousVote ? string.Empty : "переголосовать нельзя";
            isExtendPossible = response.Attributes.IsExtendPossible ? "возможно продление" : string.Empty;
            resultsOnlyAfterCompletion = response.ResultAttributes.ResultsOnlyAfterCompletion ? "результаты по окончании" : string.Empty;

            isVotingAccessible = response.IsVotingAccessible;

            answersMetas = response.AnswersMetas;

            userVoice = new List<int>(response.AnswersMetas.Count);

            //if (response.UrerVoice.Count > 0)
            //{
            //    if (response.Type == VoteType.PreferVote)
            //    {
            //        for (int i = 0; i < response.UrerVoice.Count; i++)
            //        {
            //            userVoice[response.UrerVoice[i]] = response.UrerVoice.Count - 1 - i;
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < response.UrerVoice.Count; i++)
            //        {
            //            userVoice[response.UrerVoice[i]] = 1;
            //        }
            //    }
            //}

            isActualResultAccessible = response.IsActualResultAccessible;
            countVoters = response.CountVoters;
            simpleResults = response.SimpleResults;

            isDynamicResultAccessible =response.IsDynamicResultAccessible;
            urlToDynamic = string.Empty;

            attributesStr = $"{minRoleToVoting} {timeActiveToVote} {isAnonimousVote} {isVoiceCancellationPossible} {isExtendPossible} {resultsOnlyAfterCompletion}";
        }
    }
}
