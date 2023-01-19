using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using VoteM.Models.SystemServices;
using VoteM.Models.Users.Serializable;

namespace VoteM.Models.Users.DB
{
    public class UserInDB
    {
        public string Id = string.Empty;
        public string Nickname = string.Empty;
        public string Email = string.Empty;
        public string Password = string.Empty;
        public string SerializedUser = string.Empty;
        public UserInDB() { }
        public UserInDB(User user)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            Email = user.Email;
            Password = user.Password;

            var serializer = new XmlSerializer(typeof(SUserV1));
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, new SUserV1(user));

            SerializedUser = stringWriter.ToString();
            stringWriter.Close();
        }
        public User Construct()
        {
            XmlSerializer serializer = new(typeof(SUserV1));
                StringReader stringReader = new(SerializedUser);
            User user = new(new());
            try
            {
                var ro = serializer.Deserialize(stringReader);
                if (ro is SUserV1 sUser) { user = new(sUser); }
            }
            catch { Console.WriteLine($"{Id} User Deserialize Error"); }
            finally { stringReader.Close(); }
            return user;
        }
    }

}
