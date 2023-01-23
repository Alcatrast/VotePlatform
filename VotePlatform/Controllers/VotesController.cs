using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.Controllers
{
    public class VotesController : Controller
    {
        public ViewResult Vote(string id)
        {
            ViewBag.Title = "АБОБАААА";
            var r = Request.Headers;
            VoteId vId= new VoteId(id);
            VotesDataBaseAPI.FindById(vId,out Vote vote);
            var userId = "u1";

            ////////////////////////////////

            return View(new MainVote(new VoteMainResponse(vote,userId)));
        }
    }
}
