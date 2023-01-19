using Microsoft.Data.SqlClient;
using System.Data;
using VoteM.Models.Organizations.DB;
using VoteM.Models.Users.DB;

namespace VoteM.Models.DataBaseAPI.SystemServices
{
/*
CREATE TABLE[dbo].[Users]
(
    [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY,
    [Nickname] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(50) NOT NULL,
    [SerializedUser] NVARCHAR(MAX) NOT NULL
);
*/


    public class UserDataBase
    {
        private string ConnectionString { get; }
        public UserDataBase(string connectionString)=>ConnectionString= connectionString;
        public List<UserInDB> GetById(string id) { return GetBy("Id", id); }
        public List<UserInDB> GetByNick(string nick) { return GetBy("Nickname", nick); }
        public List<UserInDB> GetByEmail(string email) { return GetBy("Email", email ); }
        public List<UserInDB> GetBy(string key,string value)
        {
            List<UserInDB> res = new();
            SqlConnection connection = new(ConnectionString);
            connection.Open();
            SqlCommand command = new($@"SELECT * FROM Users WHERE {key}=N'{value}'", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            try
            {
                while (dataReader.Read())
                {
                    var id = Convert.ToString(dataReader["Id"]);
                    var nickname = Convert.ToString(dataReader["Nickname"]);
                    var email = Convert.ToString(dataReader["Email"]);
                    var password = Convert.ToString(dataReader["Password"]);
                    var serializedUser = Convert.ToString(dataReader["SerializedUser"]);
                    res.Add(new()
                    {
                        Id = id ?? string.Empty,
                        Nickname = nickname ?? string.Empty,
                        Email = email ?? string.Empty,
                        Password = password ?? string.Empty,
                        SerializedUser = serializedUser ?? string.Empty
                    });
                }
            }
            catch { }
            finally
            {
                if (dataReader != null && dataReader.IsClosed == false) { dataReader.Close(); }
            }
            return res;
        }
        public bool Add(UserInDB userInDB) 
        {
            SqlConnection connection = new(ConnectionString);
            connection.Open();
            SqlCommand command = new(@$"INSERT INTO [Users] (Id, Nickname, Email, Password, SerializedUser) VALUES (N'{userInDB.Id}', N'{userInDB.Nickname}', N'{userInDB.Email}', N'{userInDB.Password}', N'{userInDB.SerializedUser}')",connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool Update(UserInDB userInDB)
        {
            SqlConnection connection = new(ConnectionString);
            connection.Open();
            SqlCommand command = new(@$"UPDATE Users SET Id=N'{userInDB.Id}',Nickname=N'{userInDB.Nickname}',Email=N'{userInDB.Email}',Password=N'{userInDB.Password}', SerializedUser=N'{userInDB.SerializedUser}'  WHERE Id=N'{userInDB.Id}'",connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public int CountUsers()
        {
            SqlConnection connection = new(ConnectionString);
            connection.Open();
            SqlCommand command = new(@$"SELECT count(*) FROM Users", connection);
            var resp = Convert.ToString( command.ExecuteScalar());
            if (resp != null) { return int.Parse(resp); }
            else { return 0; }
        } 
    }
}
