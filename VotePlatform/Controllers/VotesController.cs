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

            if (vote.Type == VoteType.AloneAswer)
            {

            }
            else if(vote.Type == VoteType.SomeAnswers)
            {

            }
            else if(vote.Type == VoteType.PreferVote)
            {

            }

            System.IO.File.AppendAllLines(@"c:\temp\bb.txt", new List<string>() { $"{Request.Form["vote"][0]}" });


            return View(new MainVote(new VoteMainResponse(vote, userId))) ;
        }
    }
}
