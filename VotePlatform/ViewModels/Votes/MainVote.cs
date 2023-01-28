using System.Collections.Generic;

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
        public string minRoleToVotingStr;
        public string timeActiveToVoteStr;
        public string isAnonimousVoteStr;
        public string isVoiceCancellationPossibleStr;
        public string isExtendPossibleStr;
        public string resultsOnlyAfterCompletionStr;

        public string AttributesStr { get { return $"{minRoleToVoting} {timeActiveToVote} {isAnonimousVote} {isVoiceCancellationPossibleStr} {isExtendPossible} {resultsOnlyAfterCompletion}"; } }

        public bool isVotingAccessible;

        public List<VoteMeta> answersMetas;
        public List<int> userVoice;

        public bool isActualResultAccessible;
        public int countVoters;
        public List<int> simpleResults;

        public bool isDynamicResultAccessible;
        public string urlToDynamic;

        public bool isCancellationPossible;
        public string cancelUrl;
        public string urlToVoting;
        public string urlToVoters;

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

            if (response.Attributes.MinRoleToVoting >= RoleInOrganization.Audience) { minRoleToVotingStr = "частное голосование"; } else { minRoleToVotingStr = string.Empty; }

            if (response.Attributes.IsAlwaysActiveToVote) { timeActiveToVoteStr = ""; }
            else { timeActiveToVoteStr = (response.CreatingDateTime + response.Attributes.TimeActiveToVote).ToLongDateString(); }

            isAnonimousVoteStr = response.Attributes.IsAnonimousVote ? "анонимное голосование" : string.Empty;
            isVoiceCancellationPossibleStr = response.Attributes.IsAnonimousVote ? string.Empty : "переголосовать нельзя";
            isExtendPossibleStr = response.Attributes.IsExtendPossible ? "возможно продление" : string.Empty;
            resultsOnlyAfterCompletionStr = response.ResultAttributes.ResultsOnlyAfterCompletion ? "результаты по окончании" : string.Empty;

            isVotingAccessible = response.IsVotingAccessible;

            answersMetas = response.AnswersMetas;

            userVoice = response.UrerVoice;


            isActualResultAccessible = response.IsActualResultAccessible;
            countVoters = response.CountVoters;
            simpleResults = response.SimpleResults;

            isDynamicResultAccessible =response.IsDynamicResultAccessible;
            isCancellationPossible = response.IsCancellationAccessible;
            urlToVoting= @$"{VRoutes.Controller}{VRoutes.AVoting}?id={response.Id.Id}";
            urlToDynamic = @$"{VRoutes.Controller}{VRoutes.ADynamicView}?id={response.Id.Id}";
            urlToVoters= @$"{VRoutes.Controller}{VRoutes.AVoters}?id={response.Id.Id}&=";
            cancelUrl = $@"{VRoutes.Controller}{VRoutes.AVoting}?id={response.Id.Id}&{VRoutes.PCancell}={VRoutes.PVCancel}";

            attributesStr = $"{minRoleToVotingStr} {timeActiveToVoteStr} {isAnonimousVoteStr} {isVoiceCancellationPossibleStr} {isExtendPossibleStr} {resultsOnlyAfterCompletionStr}";
        }
    }
}
