using System.Collections.Generic;

using VoteM.Models.SystemServices;
using VoteM.Models.Users.Serializable;
using VoteM.Models.DataBaseAPI;


namespace VoteM.Models.Users
{
    public class User : IUser
    {
        public string Id { get; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Meta Meta { get; set; }
        public RoleInPlatform Role { get; private set; }
        public MembersipImGroups Membersip { get; set; }
        public User(string id, string nickname, string email, string password, string name)
        {
            //Вынос в ячейки
            Id = id;
            Nickname = nickname;
            Email = email;
            Password = password;
            Role = RoleInPlatform.User;
            //
            Meta = new Meta(name, string.Empty, string.Empty);
            Membersip = new MembersipImGroups();
        }
        public bool IsPasswordRight(string password) { return Password == password; }
        public bool ChangeRole(string givingUserId, RoleInPlatform newRole)
        {
            if (Role == newRole) { return false; }

            UsersDataBaseAPI.FindById(givingUserId,out User user);
            var sgr = (sbyte)user.Role;
            
            if (sgr < (sbyte)RoleInPlatform.Validator) { return false; }
            Role = sgr > (sbyte)newRole ? newRole : (RoleInPlatform)(sgr - 1);
            return true;
        }
        public User(SUserV1 sUser)
        {
            Id = sUser.Id;
            Nickname = sUser.Nickname;
            Email = sUser.Email;
            Password = sUser.Password;
            Role = sUser.Role;
            Meta = new(sUser.Meta);
            Membersip = new(sUser.Membersip);
        }
    }
}
