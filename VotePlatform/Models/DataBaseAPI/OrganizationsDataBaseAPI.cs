
using System;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.Organizations.Serializable;
using VotePlatform.Models.Organizations.DB;
using VotePlatform.Models.DataBaseAPI.SystemServices;

namespace VotePlatform.Models.DataBaseAPI
{
    public static class OrganizationsDataBaseAPI
    {
        private static OrganizationDataBase dataBase = new OrganizationDataBase(string.Empty);

        public static void Initialize(string connectionString) => dataBase = new OrganizationDataBase(connectionString);
        public static bool Create(string organizationNick, string createrUserId)
        {
            if (dataBase.GetByNick(organizationNick).Count != 0) { return false; }

            UsersDataBaseAPI.FindById(createrUserId, out User user);
            Organization organization = new Organization(GetId(), organizationNick, new OrganizationAttributes(), new Meta(), new OrganizationMembers(createrUserId));
            if ((sbyte)user.Role < (sbyte)RoleInPlatform.User) { return false; }
            if (dataBase.Add(new OrganizationInDB(organization)) == false) { return false; }

            user.Membersip.OwnerInGroups.Add(organization.Id);
            UsersDataBaseAPI.Update(user);
            return true;
        }
        public static bool Update(Organization organization)
        {
            if ((dataBase.GetById(organization.Id).Count != 0 && dataBase.GetByNick(organization.Nickname).Count != 0) == false) { return false; }
            return dataBase.Update(new OrganizationInDB(organization));
        }
        public static bool FindById(string id, out Organization organization)
        {
            organization = new Organization(new SOrganizationV1());
            var res = dataBase.GetById(id);
            if (res.Count == 0) { return false; }
            else { organization = res[0].Construct(); return true; }
        }
        public static bool FindByNick(string nick, out Organization organization)
        {
            organization = new Organization(new SOrganizationV1());
            var res = dataBase.GetByNick(nick);
            if (res.Count == 0) { return false; }
            else { organization = res[0].Construct(); return true; }
        }
        private static string GetId()
        {
            return $"o{Convert.ToString(dataBase.CountUsers() + 1)}";
        }
    }

}
