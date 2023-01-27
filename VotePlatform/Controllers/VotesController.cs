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
        [HttpGet]
        public async Task<ViewResult> Vote(string id)
        {
            ViewBag.Title = "АБОБАААА";
            var r = Request.Headers;
            VoteId vId= new VoteId(id);
            VotesDataBaseAPI.FindById(vId,out Vote vote);
            var userId = "u1";
            string bodyStr = string.Empty;
            // try {


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

            }
            else if(vote.Type == VoteType.PreferVote)
            {

            }

            if (isChoiceParsed)
            {
                if (vote.Voiting(userId, choices))
                {
                    VotesDataBaseAPI.Update(vote);
                }
            }
            System.IO.File.AppendAllLines(path, new List<string>() { vote.Voices[0].UserId });////////////////////////
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
            
            System.IO.File.AppendAllLines(path, strings);
            
            choices[number] = 1; return true;
        }
    }
}
