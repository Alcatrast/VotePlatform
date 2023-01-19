using System;

using VotePlatform.Models.Users;
using VotePlatform.Models.Users.Serializable;
using VotePlatform.Models.Users.DB;
using VotePlatform.Models.DataBaseAPI.SystemServices;

namespace VotePlatform.Models.DataBaseAPI
{
    public static class UsersDataBaseAPI
    {
        private static UserDataBase dataBase = new UserDataBase(string.Empty);

        public static void Initialize(string connectionString) => dataBase = new UserDataBase(connectionString);

        public static bool Create(string nick, string email, string name, string password)
        {
            if (dataBase.GetByEmail(email).Count != 0 || dataBase.GetByNick(nick).Count != 0) { return false; }
            User user = new User(GetId(), nick, email, password, name);
            dataBase.Add(new UserInDB(user));
            return true;
        }
        public static bool FindById(string id, out User user)
        {
            user = new User(new SUserV1());
            var res = dataBase.GetById(id);
            if (res.Count == 0) { return false; }
            else { user = res[0].Construct(); return true; }
        }
        public static bool FindByNick(string nick, out User user)
        {
            user = new User(new SUserV1());
            var res = dataBase.GetByNick(nick);
            if (res.Count == 0) { return false; }
            else { user = res[0].Construct(); return true; }
        }

        public static bool Update(User user)
        {
            if ((dataBase.GetById(user.Id).Count != 0 && dataBase.GetByEmail(user.Email).Count != 0 && dataBase.GetByNick(user.Nickname).Count != 0) == false) { return false; }
            return dataBase.Update(new UserInDB(user));
        }
        private static string GetId() { return $"u{Convert.ToString(dataBase.CountUsers() + 1)}"; }
    }

}
