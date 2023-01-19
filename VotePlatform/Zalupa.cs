//using System.Xml.Serialization;


//using VoteM.Models.Votes;
//using VoteM.Models.SystemServices;
//using System.Text.RegularExpressions;
//using VoteM.Models.Organizations.Serializable;
//using VoteM.Models.Organizations;
//using VoteM.Models.Votes.Serializable;
//using VoteM.Models.Users.Serializable;
//using VoteM.Models.Users;
//using VoteM.Models.Votes.DB;
//using VoteM.Models.Organizations.DB;
//using VoteM.Models.Users.DB;
//using VoteM.Models.DataBaseAPI;
//using VoteM.Models.DataBaseAPI.SystemServices;
//using Microsoft.Data.SqlClient;
//using VoteM.Models.SystemServices.Serializable;
//using Microsoft.IdentityModel.Protocols.OpenIdConnect;

//namespace VoteM
//{
//    internal class Program
//    {
//        private static void Startup()
//        {
//            SqlConnection connection = new(ConnectionString);
//            connection.Open();
//            SqlCommand cu = new(@"CREATE TABLE[dbo].[Users] ( [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY, [Nickname] NVARCHAR(50) NOT NULL, [Email] NVARCHAR(50) NOT NULL, [Password] NVARCHAR(50) NOT NULL, [SerializedUser] NVARCHAR(MAX) NOT NULL );", connection);
//            SqlCommand co = new(@"CREATE TABLE[dbo].[Organizations] ( [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY, [Nickname] NVARCHAR(50) NOT NULL, [SerializedOrganization] NVARCHAR(MAX) NOT NULL );", connection);
//            SqlCommand cv = new(@"CREATE TABLE[dbo].[Votes] ( [OrganizationId] NVARCHAR (50)  NOT NULL, [IndexIn] NVARCHAR(50) NOT NULL, [SerializedVote] NVARCHAR(MAX) NOT NULL );", connection);
//            try { cu.ExecuteNonQuery(); } catch {}
//            try { co.ExecuteNonQuery(); } catch { }
//            try { cv.ExecuteNonQuery(); } catch { }

//            connection.Close();
//            Console.WriteLine("Tables created.");

//            UsersDataBaseAPI.Initialize(ConnectionString);
//            OrganizationsDataBaseAPI.Initialize(ConnectionString);
//            VotesDataBaseAPI.Initialize(ConnectionString);
            
//            UsersDataBaseAPI.Create("@owner", "owner@gmail.com", "Owner", "OWNER");
//            UsersDataBaseAPI.FindByNick("@owner",out User owner);
//            SUserV1 sOwnerV1 = new(owner)
//            {
//                Role = RoleInPlatform.Owner
//            };
//            UsersDataBaseAPI.Update(new(sOwnerV1));

//            UsersDataBaseAPI.Create("@admin", "admin@gmail.com", "Admin", "ADMIN");
//            UsersDataBaseAPI.FindByNick("@admin", out User admin);
//            admin.ChangeRole(sOwnerV1.Id, RoleInPlatform.Admin);
//            UsersDataBaseAPI.Update(admin);
            
//            UsersDataBaseAPI.Create("@validator", "validator@gmail.com", "Validator", "VALIDATOR");
//            UsersDataBaseAPI.FindByNick("@admin", out User validator);
//            validator.ChangeRole(admin.Id, RoleInPlatform.Validator);
//            UsersDataBaseAPI.Update(validator);

//            UsersDataBaseAPI.Create("@user", "user@gmail.com", "User", "USER");
//            Console.WriteLine("Users added.");

//            OrganizationsDataBaseAPI.Create("#organization", "u4");
//            OrganizationsDataBaseAPI.FindByNick("#organization", out Organization organization);
//            organization.ChangeMeta(new Meta(new SMetaV1() { Name="Organization"}),"u4");
//            OrganizationsDataBaseAPI.Update(organization);
//            Console.WriteLine("Organizations added.");

//        }
//        public static void SAV()
//        {
//            VotesDataBaseAPI.Create("u4", "o1", VoteType.AloneAswer,
//               new(new TimeSpan(), false, true, false, false, RoleInOrganization.Passerby),
//               new(false, RoleInOrganization.Passerby, RoleInOrganization.Passerby),
//               new("Vote", "This vote exist for debug."),
//               new(){ new("Answer 1", "Answer 1 exist for debug."), new("Answer 2", "Answer 2 exist for debug."),
//                new("Answer 3", "Answer 3 exist for debug."), new("Answer 4","Answer 4 exist for debug.") }
//               );
//            Console.WriteLine("Votes added");
//        }

//        public static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\RIMUCHER\Desktop\VoteM\VoteM\Database1.mdf;Integrated Security=True";
//        static void Main()
//        {            
//            Console.WriteLine("Hello, World!");
//            Startup();
//            Console.WriteLine("Startup completed");

//             // SAV();

//            // OrganizationsDataBaseAPI.FindById("o1", out Organization organization);
//            // var vts = VotesDataBaseAPI.FindAllBelongingTo(organization,RoleInOrganization.Audience,RoleInPlatform.User);
//            VotesDataBaseAPI.FindById(new("o1", "v1"), out Vote vote1);
//            vote1.Voiting("u4", new() { 0 });
//              //vote1.Voiting("u4", new() { -1 });
//            VotesDataBaseAPI.Update(vote1);
//            foreach (var it in vote1.Voices)
//            {
//                Console.WriteLine(it.UserId);
//            }
//        }

//    }
//}