using System.Collections.Generic;
using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Votes.Responses
{
    public class VotersResponse
    {
        public string AnswerHeader { get; } = "Доступ запрещён.";
        public List<string> UsersIds { get; }
        public List<int> Weights { get; }
        public VotersResponse(Vote vote, int answerIndex, string userId)
        {
            UsersIds = new List<string>();
            Weights = new List<int>();

            if (vote.IsAvailable)
            {
                if ((vote.IsAccessAllowed(userId, vote.ResultAttributes.MinRoleToActual) && (vote.Attibutes.IsAnonimousVote == false)) == false) { return; }
            }
            else
            {
                if ((vote.IsAccessAllowed(userId, RoleInOrganization.Admin) && (vote.Attibutes.IsAnonimousVote == false)) == false) { return; }
            }

            AnswerHeader = vote.AnswersMetas[answerIndex].Header;


            List<string> idsChecker = new List<string>();

            for (int i = vote.Voices.Count - 1; i >= 0; i--)
            {
                if (idsChecker.Contains(vote.Voices[i].UserId)) { continue; }
                else
                {
                    if (vote.Voices[i].Answer[answerIndex] > 0) { UsersIds.Add(vote.Voices[i].UserId); Weights.Add(vote.Voices[i].Answer[answerIndex]); }
                    idsChecker.Add(vote.Voices[i].UserId);
                }
            }
            if (vote.Type != VoteType.PreferVote)
            {
                Weights = new List<int>();
            }
        }
    }
}
