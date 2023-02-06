using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VotePlatform.Models;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users.Responses;
using VotePlatform.Models.Users;
using VotePlatform.Models.Votes.Responses;
using VotePlatform.Models.Votes;
using VotePlatform.ViewModels.Users;
using VotePlatform.ViewModels.Votes;
using System.Collections.Generic;
using System;

namespace VotePlatform.Controllers
{
    public class VotesController : Controller
    {

        public ViewResult ChangePin(string id, string pinPrefState)
        {
            ViewBag.Title = "АБОБАААА";

            VoteId vId = new VoteId(id);
            VotesDataBaseAPI.FindById(vId, out Vote vote);
            var userId = GetUserId();//"u1";
            ////////////////////////////////////////////////////////////
            bool bb = false;
            if (pinPrefState == VRoutes.PVPin) { bb = vote.Pin(userId); }
            else if (pinPrefState == VRoutes.PVUnpin) { bb = vote.Unpin(userId); }
            if (bb) { VotesDataBaseAPI.Update(vote); }
            return View("Voting", new MainVote(new VoteMainResponse(vote, userId)));
        }

        public ViewResult Create(string organizationId, string countAnswers)
        {
            int.TryParse(countAnswers, out var countAnswerNum);
            if (countAnswerNum > 64) { countAnswerNum = 64; }
            return View(new PreprocessorVoteSettings(organizationId, countAnswerNum));
        }

        [HttpPost]
        public ViewResult Creating(string organizationId, string countAnswers)
        {
            //organizationId = "o1";
            //group/user/check

            string userId = GetUserId();//"u4";
            ViewBag.Title = "АБОБАААА";
            int.TryParse(countAnswers, out var countAnswersNum);
            string id = ConstructVote(userId, organizationId, countAnswersNum);
            VotesDataBaseAPI.FindById(new VoteId(id), out Vote vote);
            return View("Voting",new MainVote(new VoteMainResponse(vote,userId)));

        }

        public ViewResult Voting(string id, string cancel)
        {
            ViewBag.Title = "АБОБАААА";

            VoteId vId = new VoteId(id);
            VotesDataBaseAPI.FindById(vId, out Vote vote);
            var userId = "u1";
            //////////////////////////////////////////////////

            if (cancel == VRoutes.PVCancel)
            {
                if (vote.Voting(userId, new List<int>() { -1 }))
                {
                    VotesDataBaseAPI.Update(vote);
                }
            }
            else
            {
                try
                {
                    bool isChoiceParsed = false;
                    List<int> choices = new List<int>();

                    if (vote.Type == VoteType.AloneAswer)
                    {
                        isChoiceParsed = TryParseAnoneChoice(vote, out choices);
                    }
                    else if (vote.Type == VoteType.SomeAnswers)
                    {
                        isChoiceParsed = TryParseSomeAnswersChoices(vote, out choices);
                    }
                    else if (vote.Type == VoteType.PreferVote)
                    {
                        isChoiceParsed = TryParsePreferVote(vote, out choices);
                    }

                    if (isChoiceParsed)
                    {
                        if (vote.Voting(userId, choices))
                        {
                            VotesDataBaseAPI.Update(vote);
                        }
                    }
                }
                catch { }
            }

            return View(new MainVote(new VoteMainResponse(vote, userId)));
        }

        public ViewResult Voters(string id, string answer)
        {
            string userId = "u1";

            var vId = new VoteId(id);
            if (VotesDataBaseAPI.FindById(vId, out Vote vote) == false) { View(new MainVoters(new List<DemoUser>(), new List<int>())); }
            if (int.TryParse(answer, out int answerNum) == false) { View(new MainVoters(new List<DemoUser>(), new List<int>())); }
            if (answerNum < 0 || answerNum >= vote.AnswersMetas.Count) { View(new MainVoters(new List<DemoUser>(), new List<int>())); };

            var response = new VotersResponse(vote, answerNum, userId);
            List<DemoUser> voters = new List<DemoUser>();
            foreach (var uvid in response.UsersIds)
            {
                UsersDataBaseAPI.FindById(uvid, out User user);
                voters.Add(new DemoUser(new UserDemoResponse(user)));
            }
            return View(new MainVoters(voters, response.Weights));

        }

        private bool TryParseAnoneChoice(Vote vote, out List<int> choices)
        {
            choices = new List<int>();
            for (int i = 0; i < vote.AnswersMetas.Count; i++) { choices.Add(0); }
            bool isValueGot = Request.Form.TryGetValue("vote", out var strings);
            if (isValueGot == false) { return false; } else { if (strings.Count == 0) { return false; } }
            bool isNumber = int.TryParse(strings[0], out var number);
            if (isNumber == false) { return false; } else { if (number >= choices.Count || number < 0) { return false; } }

            //  System.IO.File.AppendAllLines(path, strings);

            choices[number] = 1; return true;
        }
        private bool TryParseSomeAnswersChoices(Vote vote, out List<int> choices)
        {
            bool bb = TryParseAnswersChoices(vote, out choices, out int countCheckboxes);
            if (bb) { return countCheckboxes < choices.Count; }
            return false;
        }
        private bool TryParsePreferVote(Vote vote, out List<int> choices)
        {
            return TryParseAnswersChoices(vote, out choices, out _);
        }
        private bool TryParseAnswersChoices(Vote vote, out List<int> choices, out int countCheckboxes)
        {
            choices = new List<int>();
            for (int i = 0; i < vote.AnswersMetas.Count; i++) { choices.Add(0); }
            countCheckboxes = 0;
            for (int i = 0; i < choices.Count; i++)
            {
                bool isValueGot = Request.Form.TryGetValue(Convert.ToString(i), out var strings);
                if (isValueGot)
                {
                    if (strings.Count == 0) { return false; }
                    bool isNumber = int.TryParse(strings[0], out var number);
                    if (isNumber == false) { return false; }
                    choices[i] = number;
                    countCheckboxes++;
                }
            }
            if (countCheckboxes == 0) { return false; }
            return true;
        }

        private string ConstructVote(string adminId, string organizationId, int countAnswers)
        {
            var defvtype = new Dictionary<string, VoteType>
            {
                { "alone", VoteType.AloneAswer },
                { "some", VoteType.SomeAnswers },
                { "prefer", VoteType.PreferVote }
            };

            var defrole = new Dictionary<string, RoleInOrganization>
            {
                { "owner", RoleInOrganization.Owner },
                { "admin", RoleInOrganization.Admin },
                { "audience", RoleInOrganization.Audience },
                { "passerby", RoleInOrganization.Passerby }
            };

            var form = Request.Form;
            if (form == null) { return "-"; }

            //parse type
            if (form.TryGetValue("votetype", out var vtype) == false) { return "-"; };
            if (vtype.Count == 0) { return "-"; }
            if (vtype[0] == null) { return "-"; }
            if (defvtype.TryGetValue(vtype[0], out VoteType type) == false) { return "-"; }
            //parse attributes
            //pare role to vote
            if (form.TryGetValue("minroletovoting", out var vmrtv) == false) { return "-"; };
            if (vmrtv.Count == 0) { return "-"; } 
            if (vmrtv[0] == null) { return "-"; }
            if (defrole.TryGetValue(vmrtv[0], out RoleInOrganization minRoleToVoting) == false) { return "-"; }
            //parse data        
            TimeSpan timeActiveToVote = new TimeSpan(0);
            bool isAlwaysActiveToVote = true;
            bool isExtendPossible = false;
            //...............................
            bool isAnonimousVote = false;
            if (form.TryGetValue("av", out _)) { isAnonimousVote = true; }
            bool isVoiceCancellationPossible = false;
            if (form.TryGetValue("prv", out _)) { isVoiceCancellationPossible = true; }

            VoteAttributes voteAttributes = new VoteAttributes(timeActiveToVote, isExtendPossible, isAlwaysActiveToVote, isAnonimousVote, isVoiceCancellationPossible, minRoleToVoting);

            //parse result attributes
            //parse dateres
            bool resultsOnlyAfterCompletion = false;
            //parse role to actual
            if (form.TryGetValue("minroletoactual", out var vmrta) == false) { return "-"; };
            if (vmrta.Count == 0) { return "-"; }
            if (vmrta[0] == null) { return "-"; }
            if (defrole.TryGetValue(vmrta[0], out RoleInOrganization minRoleToActual) == false) { return "-"; }
            //parse role to dynamic
            if (form.TryGetValue("minroletodynamic", out var vmrtd) == false) { return "-"; };
            if (vmrtd.Count == 0) { return "-"; }
            if (vmrtd[0] == null) { return "-"; }
            if (defrole.TryGetValue(vmrtd[0], out RoleInOrganization minRoleToDynamic) == false) { return "-"; }

            VoteResultAttributes voteResultAttributes = new VoteResultAttributes(resultsOnlyAfterCompletion, minRoleToActual, minRoleToDynamic);

            //parse meta
            if (form.TryGetValue("mainheader", out var vheader) == false) { return "-"; }
            if (vheader.Count == 0) { return "-"; }
            if (vheader[0] == null) { return "-"; }
            string header = vheader[0];

            if (form.TryGetValue("maindescription", out var vdescription) == false) { return "-"; }
            if (vdescription.Count == 0) { return "-"; }
            if (vdescription[0] == null) { return "-"; }
            string description = vdescription[0];

            VoteMeta meta = new VoteMeta(header, description);

            //parse answer meta
            List<VoteMeta> answerMetas = new List<VoteMeta>();
            for (int i = 0; i < countAnswers; i++)
            {
                if (form.TryGetValue(@$"answerheader_{Convert.ToString(i)}", out var vanswerHeader) == false) { return "-"; }
                if (vanswerHeader.Count == 0) { return "-"; }
                if (vanswerHeader[0] == null) { return "-"; }
                string answerHeader = vanswerHeader[0];

                if (form.TryGetValue(@$"answerdescription_{Convert.ToString(i)}", out var vanswerDescription) == false) { return "-"; }
                if (vanswerDescription.Count == 0) { return "-"; }
                if (vanswerDescription[0] == null) { return "-"; }
                string answerDescription = vanswerDescription[0];

                answerMetas.Add(new VoteMeta(answerHeader, answerDescription));
            }
            return VotesDataBaseAPI.Create(adminId, organizationId, type, voteAttributes, voteResultAttributes, meta, answerMetas).Id;
        }
        private string GetUserId()
        {
            string id = string.Empty;
            try
            {
                Request.Cookies.TryGetValue("i", out var bid);
                Request.Cookies.TryGetValue("p", out var bpass);
                if (UsersDataBaseAPI.FindById(bid, out User user))
                {
                    if (user.IsPasswordRight(bpass)) { id = bid; }
                }
            }
            catch { }
            return id;
        }
    }
}