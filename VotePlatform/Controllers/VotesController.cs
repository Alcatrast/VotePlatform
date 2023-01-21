using Microsoft.AspNetCore.Mvc;
using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.ViewModels.Votes;

namespace VotePlatform.Controllers
{
    public class VotesController : Controller
    {
        public ViewResult Vote()
        {
            var r = Request.Headers;
            var dv = new DemoVote(new VoteDemoResponse(new Vote(new SVoteV1() { Meta=new SVoteMetaV1() { Header="да.", Description="я смог это."} })));
            return View(dv);
        }
    }
}
