using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.Organizations.Serializable;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users;
using VotePlatform.Models.Users.DB;
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

            string userId = GetUserId();//"u4";
            /////////////////////////////////////////////////
            return View(new MainOrganization(new OrganizationMainResponse(organization, userId)));
        }

        public ViewResult Audience(string id)
        {
            OrganizationsDataBaseAPI.FindById(id, out Organization organization);
            string userId = GetUserId();//"u4";
            /////////////////////////////////////////////////

            PeopleInOrganization peopleInOrganization = new PeopleInOrganization("Участники", new List<string>(), false, false, false, false, false);
            if ((sbyte)organization.GetRoleInOrganization(userId) >= (sbyte)RoleInOrganization.Admin)
            {
                var audience = organization.Members.Audience;
                if ((sbyte)organization.GetRoleInOrganization(userId) >= (sbyte)RoleInOrganization.Owner)
                {
                    peopleInOrganization = new PeopleInOrganization("Участники", audience, true, false, false, true, false);
                }
                else
                {
                    peopleInOrganization = new PeopleInOrganization("Участники", audience, false, false, false, true, false);
                }
            }
            else
            {
                var audience = organization.Members.AllMembers;
                if ((sbyte)organization.GetRoleInOrganization(userId) >= (sbyte)RoleInOrganization.Audience)
                {
                    peopleInOrganization = new PeopleInOrganization("Участники", audience, false, false, false, false, false);
                }
            }

            return View("People", peopleInOrganization);
        }
        public ViewResult Admins(string id)
        {
            OrganizationsDataBaseAPI.FindById(id, out Organization organization);
            string userId = GetUserId();//"u4";
            /////////////////////////////////////////////////


            PeopleInOrganization peopleInOrganization = new PeopleInOrganization("Администраторы", new List<string>(), false, false, false, false, false);
            if ((sbyte)organization.GetRoleInOrganization(userId) >= (sbyte)RoleInOrganization.Owner)
            {
                var admins = organization.Members.Admins;
                peopleInOrganization = new PeopleInOrganization("Администраторы", admins, false, true, false, false, false);
            }
            return View("People", peopleInOrganization);
        }
        public ViewResult Queue(string id)
        {
            OrganizationsDataBaseAPI.FindById(id, out Organization organization);
            string userId = GetUserId();//"u4";
            /////////////////////////////////////////////////


            PeopleInOrganization peopleInOrganization = new PeopleInOrganization("Очередь", new List<string>(), false, false, false, false, false);
            var queue = organization.Members.Queue;
            if ((sbyte)organization.GetRoleInOrganization(userId) >= (sbyte)RoleInOrganization.Admin)
            {
                peopleInOrganization = new PeopleInOrganization("Очередь", queue, false, false, true, false, true);
            }
            return View("People", peopleInOrganization);
        }
        public ViewResult ApplicationForMembership(string id)
        {
            string userId = GetUserId();//"u1";
            //////
            OrganizationsDataBaseAPI.FindById(id, out Organization organization);
            if (organization.ApplicationForMembership(userId))
            {
                OrganizationsDataBaseAPI.Update(organization);
            }
            return Main(id);
        }
        public ViewResult Create()
        {
            return View();
        }
        public ViewResult Creating()
        {
            /////////
            string userId = GetUserId();//"u4";
            try
            {
                if(Request.Form.TryGetValue("nick",out var nicks)&&Request.Form.TryGetValue("name",out var names)) 
                {
                    if (nicks.Count>0 && names.Count>0) 
                    {
                        if (nicks[0]!=null && names[0]!=null) 
                        {
                            if (OrganizationsDataBaseAPI.Create($@"#{nicks[0]}",userId))
                            {
                                OrganizationsDataBaseAPI.FindByNick($@"#{nicks[0]}", out Organization organization);
                                organization.ChangeMeta(new Meta($"{names[0]}"), userId);
                                OrganizationsDataBaseAPI.Update(organization);
                                return View("Main",new MainOrganization(new OrganizationMainResponse(organization,userId)));
                            }
                        }
                    }
                }
            }
            catch { }
            return View("Main", new MainOrganization(new OrganizationMainResponse(new Organization(new SOrganizationV1()),"")));
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