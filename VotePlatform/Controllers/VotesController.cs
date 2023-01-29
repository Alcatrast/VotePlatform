using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Responses;
using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.Responses;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.ViewModels.Users;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.Controllers
{
    public class VotesController : Controller
    {
        public string path = @"C:\temp\bb.txt";

        public ViewResult Voting(string id, string cancel)
        {
            ViewBag.Title = "АБОБАААА";

            VoteId vId = new VoteId(id);
            VotesDataBaseAPI.FindById(vId, out Vote vote);
            var userId = "u1";

            if (cancel == VRoutes.PVCancel) 
            {
                if(vote.Voiting(userId, new List<int>() { -1 }))
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
                        if (vote.Voiting(userId, choices))
                        {
                            VotesDataBaseAPI.Update(vote);
                        }
                    }
                }
                catch { }
            }

            return View(new MainVote(new VoteMainResponse(vote, userId))) ;
        }

        public ViewResult Voters(string id, string answer)
        {
            string userId = "u1";

            var vId = new VoteId(id);
            if (VotesDataBaseAPI.FindById(vId, out Vote vote) == false) { View(new MainVoters(new List<DemoUser>(), new List<int>())); }
            if (int.TryParse(answer, out int answerNum) == false) { View(new MainVoters(new List<DemoUser>(), new List<int>())); }
            if (answerNum < 0 || answerNum >= vote.AnswersMetas.Count) { View(new MainVoters(new List<DemoUser>(), new List<int>())); };

            var response= new VotersResponse(vote, answerNum, userId);
            List<DemoUser> voters = new List<DemoUser>();
            foreach(var uvid in response.UsersIds)
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
            if (isValueGot == false) { return false; } else { if(strings.Count==0) { return false; } }
            bool isNumber = int.TryParse(strings[0],out var number);
            if(isNumber==false) { return false; } else { if(number>=choices.Count || number<0) { return false; } }
            
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
        private bool TryParseAnswersChoices(Vote vote,out List<int> choices, out int countCheckboxes)
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
            if(countCheckboxes ==0) { return false; }
            return true;    
        }

    }
}
