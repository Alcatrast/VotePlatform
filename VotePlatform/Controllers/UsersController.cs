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
using VotePlatform.Models.Organizations;
using VotePlatform.ViewModels.Organizations;
using VotePlatform.Models.Users.Serializable;
using Azure.Core;
using Azure;

namespace VotePlatform.Controllers
{
    public class UsersController : Controller
    {
        public ViewResult Main(string id)
        {
            UsersDataBaseAPI.FindById(id, out User user);

            string myId = GetUserId();//"u1";

            return View(new MainUser(new UserMainResponse(user, myId)));
        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Creating()
        {
            try
            {
                if (Request.Form.TryGetValue("nick", out var nicks) && Request.Form.TryGetValue("name", out var names) && Request.Form.TryGetValue("email", out var emails) && Request.Form.TryGetValue("pass", out var passs))
                {
                    if (nicks.Count > 0 && names.Count > 0 && emails.Count > 0 && passs.Count > 0)
                    {
                        if (nicks[0] != null && names[0] != null && emails[0] != null && passs[0] != null)
                        {
                            if (UsersDataBaseAPI.Create($"@{nicks[0]}", names[0], emails[0], passs[0]))
                            {
                                UsersDataBaseAPI.FindByNick($"@{nicks[0]}", out User user);
                                Response.Cookies.Append("i", user.Id);
                                Response.Cookies.Append("p", passs[0]);
                                return View("Main", new MainUser(new UserMainResponse(user, user.Id)));
                            }
                        }
                    }
                }
            }
            catch { }
            return View("Main", new MainUser(new UserMainResponse(new User(new SUserV1()), string.Empty)));
        }

        public ViewResult SingIn()
        {
            return View();
        }
        [HttpPost]
        public ViewResult SingingIn()
        {
            try
            {
                if (Request.Form.TryGetValue("nick", out var nicks) && Request.Form.TryGetValue("pass", out var passs))
                {
                    if (nicks.Count > 0 && passs.Count > 0)
                    { 
                        if (nicks[0] != null && passs[0] != null)
                        {
                            if (UsersDataBaseAPI.FindByNick($"@{nicks[0]}",out User user))
                            {
                                if (user.IsPasswordRight(passs[0]))
                                {
                                   // try {
                                        Response.Cookies.Append("i", user.Id);
                                        Response.Cookies.Append("p", passs[0]);
                                   //}catch {  }
                                    return View("Main", new MainUser(new UserMainResponse(user, user.Id)));
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return View("Main", new MainUser(new UserMainResponse(new User(new SUserV1()), string.Empty)));
        }

        private string GetUserId()
        {
            string id = string.Empty;
            try
            {
                Request.Cookies.TryGetValue("i", out var bid);
                Request.Cookies.TryGetValue("p", out var bpass);
                if(UsersDataBaseAPI.FindById(bid, out User user))
                {
                    if(user.IsPasswordRight(bpass)) { id = bid; }
                }
            }
            catch { }
            return id;
        }

    }
}
