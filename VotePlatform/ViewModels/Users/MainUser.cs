using System.Collections.Generic;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Responses;
using VotePlatform.ViewModels.Organizations;

namespace VotePlatform.ViewModels.Users
{
    public class MainUser
    {
        public string nickname;
        public string email;
        public Meta meta;
        public string role;
        public List<DemoOrganization> ownedOrg;
        public List<DemoOrganization> adminedOrg;
        public List<DemoOrganization> audiencedOrg;

        public MainUser(UserMainResponse response) 
        {
            nickname=response.Nickname;
            email=response.Email;
            meta=response.Meta;

            role = response.Role switch
            {
                RoleInPlatform.User => "Пользователь",
                RoleInPlatform.Validator => "Валидатор",
                RoleInPlatform.Admin => "Админ",
                RoleInPlatform.Owner => "Владелец",
                RoleInPlatform.Passerby => "Заблокирован",
                _ => "Заблокирован",
            };

            ownedOrg = new List<DemoOrganization>();
            adminedOrg = new List<DemoOrganization>();  
            audiencedOrg= new List<DemoOrganization>();
            foreach(var oId in response.Membersip.OwnerInGroups)
            {
                OrganizationsDataBaseAPI.FindById(oId, out Organization organization);
                ownedOrg.Add(new DemoOrganization(new OrganizationDemoResponse(organization)));
            }
            foreach (var oId in response.Membersip.AdminInGroups)
            {
                OrganizationsDataBaseAPI.FindById(oId, out Organization organization);
                adminedOrg.Add(new DemoOrganization(new OrganizationDemoResponse(organization)));
            }
            foreach (var oId in response.Membersip.AudienceInGtoups)
            {
                OrganizationsDataBaseAPI.FindById(oId, out Organization organization);
                audiencedOrg.Add(new DemoOrganization(new OrganizationDemoResponse(organization)));
            }

        }
    }
}
