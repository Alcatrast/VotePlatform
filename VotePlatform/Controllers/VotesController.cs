using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.Controllers
{
    public class VotesController : Controller
    {
        public string path = @"C:\temp\bb.txt";
        //[HttpGet]
        public async Task<ViewResult> Vote(string id)
        {
            ViewBag.Title = "АБОБАААА";
            var r = Request.Headers;
            VoteId vId= new VoteId(id);
            VotesDataBaseAPI.FindById(vId,out Vote vote);
            var userId = "u1";
            string bodyStr = string.Empty;
            


            return View(new MainVote(new VoteMainResponse(vote, userId))) ;
        }

        //[HttpPost]
        public async Task<ViewResult> Voting(string id)
        {
            ViewBag.Title = "АБОБАААА";

            VoteId vId = new VoteId(id);
            VotesDataBaseAPI.FindById(vId, out Vote vote);
            var userId = "u1";

            bool isChoiceParsed = false;
            List<int> choices= new List<int>();

            if (vote.Type == VoteType.AloneAswer)
            {
                isChoiceParsed = TryParseAnoneChoice(vote, out choices);
            }
            else if(vote.Type == VoteType.SomeAnswers)
            {
                isChoiceParsed = TryParseSomeAnswersChoices(vote, out choices);
            }
            else if(vote.Type == VoteType.PreferVote)
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
            return View(new MainVote(new VoteMainResponse(vote, userId))) ;
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
