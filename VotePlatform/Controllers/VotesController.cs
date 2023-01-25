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
        public async Task<ViewResult> Voting(string id,string vote)
        {
            ViewBag.Title = "АБОБАААА";
            var r = Request.Headers;
            VoteId vId = new VoteId(id);
            VotesDataBaseAPI.FindById(vId, out Vote votje);
            var userId = "u1";
            string bodyStr = string.Empty;
            // try {

            //bodyStr = await (new StreamReader(Request.Body)).ReadToEndAsync();
           // System.IO.File.AppendAllLines(@"c:\temp\bb.txt", new List<string>() { $" gg {bodyStr}" });


            //}catch { }
            //var body = QueryHelpers.ParseQuery(bodyStr);
            System.IO.File.AppendAllLines(@"c:\temp\bb.txt", new List<string>() { $"{vote}" });

            //foreach (var item in body)
            //{
            //    System.IO.File.AppendAllLines(@"c:\temp\bb.txt", new List<string>() { $"{item.Key} {item.Value}" });
            //}
            //if (body.ContainsKey("vote")) { throw new NotImplementedException(); }

            return View(new MainVote(new VoteMainResponse(votje, userId))) ;
        }
    }
}
