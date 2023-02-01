using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Responses;
using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.Responses;
using VotePlatform.ViewModels.Organizations;
using VotePlatform.ViewModels.Users;
using VotePlatform.ViewModels.Votes;
namespace VotePlatform.Controllers
{
    public class OrganizationsController : Controller
    {
        public ViewResult Main(string id)
        {
            OrganizationsDataBaseAPI.FindById(id, out Organization organization);
            
           return View(new MainOrganization(new OrganizationMainResponse(organization,"u4")));
        }
    }
}
